using Scrapper.Dates.Model;

namespace Scrapper.Dates.Managers
{
    public class DescriptionBasedQuestion
    {
        private readonly int _difficulty;
        public string QuestionTitle => "W którym roku miało miejsce poniższe wydarzenie:";
        public string QuestionBody { get; set; }
        public string CorrectAnswer { get; set; }
        public string SearchText { get; set; }
        public int SuggestedDifficulty { get; set; }
        public DescriptionBasedQuestion(HistoricalFact fact, List<HistoricalFact> fakes, int difficultyLevel)
        {
            SuggestedDifficulty = difficultyLevel;
            _difficulty = SuggestedDifficulty * 10;
            CorrectAnswer = fact.Year.ToString();
            SearchText = fact.DescriptionOfFact;
            List<string> possibleAnswers = fakes.Where(w => w.DateOfFact.Year != fact.DateOfFact.Year
                                                            && w.Year >= fact.Year - _difficulty && w.Year <= fact.Year + _difficulty)
                                                .OrderBy(o => Guid.NewGuid())
                                                .Select(s => s.Year.ToString())
                                                .Distinct()
                                                .Take(3)
                                                .ToList();         
            possibleAnswers.Add(CorrectAnswer);
            possibleAnswers = possibleAnswers.OrderBy(o => Guid.NewGuid()).ToList();
            QuestionBody = GenerateQuestionBodyHtml(possibleAnswers);
        }
        private string GenerateQuestionBodyHtml(List<string> ansewrs)
        {
            string html = $@"<p>{SearchText}<br>&nbsp;</p>";
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
