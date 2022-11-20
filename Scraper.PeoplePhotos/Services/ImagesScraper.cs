using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Scraper.PeoplePhotos.Dtos;
using Scraper.PeoplePhotos.Entities;
using Image = Scraper.PeoplePhotos.Entities.Image;
using FaceRecognitionDotNet;
using System.Net;
using System.Text.RegularExpressions;
using Mosaik.Core;
using Catalyst;
using IronOcr;

namespace Scraper.PeoplePhotos.Services
{
    public interface IImagesScraper
    {
        void Scrap();
    }

    public class ImagesScraper : IImagesScraper
    {
        private readonly ImagesContext _context;
        private FaceRecognition faceRecognition;
        private IronTesseract ocr;
        private HashSet<string> _maleFirstNames;
        private HashSet<string> _famaleFirstNames;

        public ImagesScraper(ImagesContext imagesContext)
        {
            _context = imagesContext;
        }

        public void Scrap()
        {
            Console.WriteLine("Start scraping");

            //Catalyst.Models.Polish.Register();

            //var doc = new Document("To jest zdjęcie Józefa Piłsudzkiego. To jest Józef Połsudzki.", Language.Polish);
            //var nlp = Pipeline.For(Language.Polish);
            //nlp.ProcessSingle(doc);

            _maleFirstNames = File.ReadAllLines("Data/polish_male_firstnames.txt").Select(x => x.ToLower()).ToArray().ToHashSet();
            _famaleFirstNames = File.ReadAllLines("Data/polish_female_firstnames.txt").Select(x => x.ToLower()).ToArray().ToHashSet();
            faceRecognition = FaceRecognition.Create("FaceRecognition");
            ocr = new IronTesseract(); // nothing to configure
            ocr.Language = OcrLanguage.PolishBest;
            ocr.Configuration.TesseractVersion = TesseractVersion.Tesseract5;
            IWebDriver driver = new ChromeDriver();
            int page = 0; //124
            while (true)
            {
                driver.Url = $"https://przystanekhistoria.pl/pa2/teksty?page={page}";

                List<ImageDto> imageWithAlts = new();

                HashSet<string> visitedSites = new();

                HashSet<string> sitesToVisit = driver
                    .FindElements(By.CssSelector("body > div.container-m > div > a[href]"))
                    .Select(x => x.GetAttribute("href"))
                    .ToHashSet();

                foreach (var siteToVisit in sitesToVisit)
                {
                    driver.Url = siteToVisit;
                    visitedSites.Add(siteToVisit);

                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    var x = wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                    var imagesFromSite = driver.FindElements(By.CssSelector(".article-container img[src][alt]")).Select(x => new ImageDto
                    {
                        Url = x.GetAttribute("src").Trim(),
                        Alt = x.GetAttribute("alt").Trim()
                    });

                    foreach (var image in imagesFromSite)
                    {
                        AddImage(image);
                    }

                    Thread.Sleep(500);
                }

                Thread.Sleep(500);
                page++;
            }

            Console.WriteLine("End scraping");
        }

        public void AddImage(ImageDto imageDto)
        {
            Image image = _context.Images.FirstOrDefault(x => x.Url == imageDto.Url);

            if (image == null)
            {
                image = new Image
                {
                    Url = imageDto.Url,
                    Alt = imageDto.Alt,
                };

                _context.Images.Add(image);
            }
            _context.SaveChanges();

            List<PersonDto> people = FindPeopleInAlt(image.Alt);
            if (people.Count != 0)
            {
                foreach (PersonDto person in people)
                {
                    int personId = AddOrGetPersonId(person);

                    if (!image.People.Any(x => x.PersonId == personId))
                    {
                        image.People.Add(new ImagePerson
                        {
                            PersonId = personId
                        });
                    }
                }
            }

            if (image.FacesCount == null)
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(new Uri(image.Url), @"FaceRecognition/image.png");
                    }
                    var faceRecognitionImage = FaceRecognition.LoadImageFile("FaceRecognition/image.png");
                    image.FacesCount = faceRecognition.FaceLocations(faceRecognitionImage).Count();
                }
                catch
                {

                }
            }

            try
            {
                using (var Input = new OcrInput())
                {
                    Input.AddImage(@"FaceRecognition/image.png");
                    var ocrResult = ocr.Read(Input);
                    image.Ocr = ocrResult.Text?.Trim();
                }
            }
            catch
            {

            }

            _context.SaveChanges();
        }

        public int AddOrGetPersonId(PersonDto personDto)
        {
            Person? person = _context.People.FirstOrDefault(x => x.Name == personDto.Name);
            if (person == null)
            {
                person = new Person()
                {
                    Name = personDto.Name,
                    Sex = personDto.Sex
                };

                _context.People.Add(person);
                _context.SaveChanges();
            }

            return person.Id;
        }

        public List<PersonDto> FindPeopleInAlt(string alt)
        {
            List<string> potencialNames = Regex.Matches(alt, @"[A-ZŻŹĆĄŚĘŁÓŃ]{1}[a-zżźćńółęąś]*\s[A-ZŻŹĆĄŚĘŁÓŃ]{1}[a-zżźćńółęąś]*")
                .Select(x => x.Value)
                .ToList();

            List<PersonDto> people = new();

            foreach (string potencialName in potencialNames)
            {
                string[] splittedBySpace = potencialName.Split(' ');

                if (splittedBySpace.Length < 2)
                {
                    continue;
                }

                string lowerFirstName = splittedBySpace[0].ToLower();
                string lowerLastName = splittedBySpace[1].ToLower();

                if (lowerFirstName.Length <= 3)
                {
                    continue;
                }

                if (lowerLastName.Length <= 3)
                {
                    continue;
                }

                if (_maleFirstNames.Contains(lowerFirstName))
                {
                    people.Add(new PersonDto
                    {
                        Name = potencialName,
                        Sex = Sex.MALE
                    });
                }
                else if (_famaleFirstNames.Contains(lowerFirstName))
                {
                    if (lowerFirstName.EndsWith('a'))
                    {
                        string lowerFirstNameWithOutLastA = lowerFirstName.Substring(0, lowerFirstName.Length - 1);
                        if (_maleFirstNames.Contains(lowerFirstNameWithOutLastA))
                        {
                            //Pomijamy problematyczne imiona
                            continue;
                        }
                    }

                    people.Add(new PersonDto
                    {
                        Name = potencialName,
                        Sex = Sex.FAMALE
                    });
                }
            }

            return people;
        }
    }
}
