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
                ImagePerson? imagePerson = person.Images.FirstOrDefault(x => x.Image.FacesCount == 1 && x.Image.People.Count == 1);

                if (imagePerson == null)
                {
                    continue;
                }

                Sex sex = person.Sex;
                List<string> fakeAnswers = _context.People
                    .Where(x => x.Sex == sex && x.Name != person.Name)
                    .Select(x => x.Name)
                    .Take(3)
                    .ToList();

                List<string> answers = fakeAnswers.ToList();
                answers.Add(person.Name);
                answers.OrderBy(x => Guid.NewGuid());

                QuestionDto question = new()
                {
                    QuestionTitle = "Jaka postać przedstawiona jest na zdjęciu?",
                    QuestionBody = $"<img src='{imagePerson.Image.Url}'><br>{string.Join("<br> -",answers)}",
                    SearchText = person.Name,
                    CorrectAnswer = person.Name,
                    SuggestedDifficulty = 1
                };
                questions.Add(question);
            }

            var questionsStringContent = new StringContent(JsonConvert.SerializeObject(questions), Encoding.UTF8, "application/json");
            _ = new HttpClient().PostAsync("https://localhost:7068/api/questions/save", questionsStringContent).Result;
        }
    }
}
