using Scrapper.Dates.Model;
using System.Net;

namespace Scrapper.Dates.Managers
{
    public class QuestionsManager
    {
        private readonly List<HistoricalFact> _facts;
        private readonly int[] _difficulties = { 1, 2, 3 };

        public QuestionsManager(List<HistoricalFact> facts)
        {
            this._facts = facts;
        }

        public IEnumerable<DescriptionBasedQuestion> GetDescriptionBasedQuestions()
        {
            foreach (var f in _facts)
            {
                foreach (var d in _difficulties)
                {
                    yield return new DescriptionBasedQuestion(f, _facts, d);

                }
            }
        }

        public IEnumerable<YearBasedQuestion> GetYearBasedQuestions()
        {
            foreach (var f in _facts)
            {
                foreach (var d in _difficulties)
                {
                    yield return new YearBasedQuestion(f, _facts, d);
                }
            }
        }
    }
}
