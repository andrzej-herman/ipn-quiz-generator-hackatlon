using Microsoft.AspNetCore.Razor.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scrapper.Dates.Model;

namespace Scrapper.Dates.Core
{
    public class WebScrapper : BackgroundService
    {
        private readonly string _url = "https://przystanekhistoria.pl/json_ph/";
        private readonly int yearFrom = 1917;
        private readonly int yearTo = 1990;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    List<HistoricalFact> resultList = new List<HistoricalFact>();
                    for (int i = yearFrom; i <= yearTo; i++)
                    {
                        var result = await client.GetAsync($@"{_url}{i}");
                        string responseBody = await result.Content.ReadAsStringAsync();
                        var a = JsonConvert.DeserializeObject<JObject>(responseBody);
                        var zx = a.GetType();


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
                                            //var obj = item5.Children()["topic"].Values<string>().First().ToString();
                                            resultList.Add(new HistoricalFact(item5, i));
                                        }
                                    }

                                }
                            }
                        }
                    }
                    
                
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
