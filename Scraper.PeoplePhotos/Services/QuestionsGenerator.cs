using FaceRecognitionDotNet;
using Microsoft.EntityFrameworkCore;
using Mosaik.Core;
using Newtonsoft.Json;
using Scraper.PeoplePhotos.Dtos;
using Scraper.PeoplePhotos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image = Scraper.PeoplePhotos.Entities.Image;

namespace Scraper.PeoplePhotos.Services
{
    public interface IQuestionsGenerator
    {
        void Generate();
    }

    public class QuestionsGenerator : IQuestionsGenerator
    {
        private readonly ImagesContext _context;

        public QuestionsGenerator(ImagesContext imagesContext)
        {
            _context = imagesContext;
        }

        public void Generate()
        {
            List<Person> people = _context.People
                .Include(x => x.Images)
                    .ThenInclude(x => x.Image)
                        .ThenInclude(x => x.People)
                .OrderByDescending(x => x.Images.Count)
                .ToList();

            List<QuestionDto> questions = new();

            foreach (Person person in people)
            {
                ImagePerson? imagePerson = person.Images.FirstOrDefault(x => x.Image.FacesCount == 1 && x.Image.Ocr == string.Empty && x.Image.People.Count == 1);

                if (imagePerson == null)
                {
                    continue;
                }

                List<string> fakeAnswers = people
                    .Where(x => x.Sex == person.Sex)
                    .Select(x => x.Name)
                    .OrderBy(x => x)
                    .SkipWhile(x => x != person.Name)
                    .Take(4)
                    .ToList();

                List<string> answers = fakeAnswers.ToList();
                if (answers.Count < 4)
                {
                    answers.AddRange(people
                        .Where(x => x.Sex == person.Sex)
                        .Select(x => x.Name)
                        .Take(4 - answers.Count));
                }
                answers = answers.OrderBy(x => Guid.NewGuid()).ToList();

                int suggestedDifficulty;
                switch (person.Images.Count)
                {
                    case > 9:
                        suggestedDifficulty = 1;
                        break;
                    case > 3:
                        suggestedDifficulty = 2;
                        break;
                    default:
                        suggestedDifficulty = 3;
                        break;
                }

                QuestionDto question = new()
                {
                    QuestionTitle = "Jaka postać przedstawiona jest na zdjęciu?",
                    QuestionBody = $"<img src='{imagePerson.Image.Url}'><br>A. {answers[0]}<br>B. {answers[1]}<br>C. {answers[2]}<br>D. {answers[3]}",
                    SearchText = imagePerson.Image.Alt,
                    CorrectAnswer = person.Name,
                    SuggestedDifficulty = suggestedDifficulty
                };
                questions.Add(question);
            }

            var questionsStringContent = new StringContent(JsonConvert.SerializeObject(questions), Encoding.UTF8, "application/json");
            _ = new HttpClient().PostAsync("https://ipn.hostingasp.pl/api/questions/save", questionsStringContent).Result;
        }
    }
}
