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
        public int DifficultyLevel { get; set; }
        public DescriptionBasedQuestion(HistoricalFact fact, List<HistoricalFact> fakes, int difficultyLevel)
        {
            DifficultyLevel = difficultyLevel;
            _difficulty = DifficultyLevel * 10;
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
            return $@"<p>{QuestionTitle}<br>&nbsp;</p>
                            <p style=""margin-left:40px;"">{QuestionBody}<br>&nbsp;</p>
                            <figure class=""table"" style=""width:500px;"">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>{ansewrs[0]}</td>
                                            <td>{ansewrs[1]}</td>
                                        </tr>
                                        <tr>
                                            <td>{ansewrs[2]}</td>
                                            <td>{ansewrs[3]}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </figure>
                            <p>&nbsp;</p>";
        }
    }
}
