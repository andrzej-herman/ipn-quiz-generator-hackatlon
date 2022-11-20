using Scrapper.Dates.Model;
using System.Globalization;

namespace Scrapper.Dates.Managers
{
    public class YearBasedQuestion
    {
        private readonly int _difficulty;
        public string QuestionTitle { get; private set; }
        public string QuestionBody { get; set; }
        public string CorrectAnswer { get; set; }
        public string SearchText { get; set; }
        public int SuggestedDifficulty { get; set; }
        public YearBasedQuestion(HistoricalFact fact, List<HistoricalFact> fakes, int difficultyLevel)
        {
            SuggestedDifficulty = difficultyLevel;
            _difficulty = SuggestedDifficulty * 10;
            QuestionTitle = $"Co wydarzyło się {fact.DateOfFact.ToString("dd MMMMM yyyy", CultureInfo.GetCultureInfo("pl-PL"))}?";
            CorrectAnswer = fact.DescriptionOfFact;
            SearchText = fact.DescriptionOfFact;
            fakes = fakes.Where(w =>  w != fact && w.Year >= fact.Year - _difficulty && w.Year <= fact.Year + _difficulty)
                            .OrderBy(o => Guid.NewGuid())
                            .Take(3)
                            .ToList();
            List<string> possibleAnswers = fakes.Select(s => s.DescriptionOfFact).ToList();
            possibleAnswers.Add(CorrectAnswer);
            possibleAnswers = possibleAnswers.OrderBy(o => Guid.NewGuid()).ToList();
            QuestionBody = GenerateQuestionBodyHtml(possibleAnswers);
        }
        private string GenerateQuestionBodyHtml(List<string> ansewrs)
        {
            string html = $@"<p>{QuestionTitle}<br>&nbsp;</p>";
            html += "<div style=\"width:100%!important\">";
            html += "<table class=\"table table-bordered w-100\">";
            html += "<tbody>";
            html += "<tr>";
            html += $"<td>A. {ansewrs[0]}</td>";
            html += $"<td>B. {ansewrs[1]}</td>";
            html += "</tr>";
            html += "<tr>";
            html += $"<td>C. {ansewrs[2]}</td>";
            html += $"<td>D. {ansewrs[3]}</td>";
            html += "</tr>";
            html += "</tbody>";
            html += "</table>";
            html += "</div>";
            return html;
        }
    }
}
