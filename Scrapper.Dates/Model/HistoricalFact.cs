using Newtonsoft.Json.Linq;

namespace Scrapper.Dates.Model
{
    public class HistoricalFact
    {
        public HistoricalFact(JToken token, int year)
        {
            DescriptionOfFact = token.Children()["topic"].Values<string>().First().ToString();
            Year = year;
            ResolvePath(token.Path);
        }
        public int Id { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime DateOfFact => new DateTime(Year, Month, Day);
        public string DescriptionOfFact { get; set; }
        private void ResolvePath(string path)
        {
            var asd = path.Split('.');
            Month = int.Parse(asd[0]);
            Day = int.Parse(asd[1]);
            Id = int.Parse(asd[2]);
        }
    }
}
