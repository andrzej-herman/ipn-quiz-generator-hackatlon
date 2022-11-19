using Scrapper.Dates.Model;

namespace Scrapper.Dates.Managers
{
    public class DescriptionBasedQuestion
    {
        public string QuestionTitle => "W którym roku wydarzyło się:";
        public string QuestionBody { get; set; }
        public string CorrectAnswer { get; set; }
        public string Search { get; set; }
        public DescriptionBasedQuestion(HistoricalFact fact, List<HistoricalFact> fakes)
        {
            CorrectAnswer = fact.Year.ToString();
            Search = fact.DescriptionOfFact;
            fakes = fakes.Where(w => w.DateOfFact.Year != fact.DateOfFact.Year).OrderBy(o => Guid.NewGuid()).Take(3).ToList();
            List<string> possibleAnswers = fakes.Select(s => s.Year.ToString()).ToList();
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
