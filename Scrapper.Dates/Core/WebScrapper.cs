using Microsoft.AspNetCore.Razor.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scrapper.Dates.Managers;
using Scrapper.Dates.Model;
using System.Reflection;
using System.Text;

namespace Scrapper.Dates.Core
{
    public class WebScrapper : BackgroundService
    {
        private readonly string _url = "https://przystanekhistoria.pl/json_ph/";
        private readonly string _exportUrl = "https://localhost:7068/api/questions/save";
        private readonly int yearFrom = 1917;
        private readonly int yearTo = 1990;
        private static HttpClient _client = new HttpClient();
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            List<HistoricalFact> resultList = new List<HistoricalFact>();
            for (int i = yearFrom; i <= yearTo; i++)
            {
                var result = await _client.GetAsync($@"{_url}{i}");
                string responseBody = await result.Content.ReadAsStringAsync();
                var a = JsonConvert.DeserializeObject<JObject>(responseBody);

                foreach (var item in a.AsJEnumerable())
                {
                    foreach (var item2 in item.AsJEnumerable())
                    {
                        foreach (var item3 in item2.AsJEnumerable())
                        {
                            foreach (var item4 in item3.AsJEnumerable())
                            {
                                foreach (var item5 in item4.AsJEnumerable())
                                {
                                    resultList.Add(new HistoricalFact(item5, i));
                                }
                            }
                        }
                    }
                }
            }

            QuestionsManager manager = new QuestionsManager(resultList);
            var dQuestions = new StringContent(JsonConvert.SerializeObject(manager.GetDescriptionBasedQuestions()), Encoding.UTF8, "application/json");
            var yQuestions = new StringContent(JsonConvert.SerializeObject(manager.GetYearBasedQuestions()), Encoding.UTF8, "application/json");
            await _client.PostAsync(_exportUrl, dQuestions);
            await _client.PostAsync(_exportUrl, yQuestions);

        }

    }
}
