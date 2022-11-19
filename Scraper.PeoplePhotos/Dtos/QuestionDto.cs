namespace Scraper.PeoplePhotos.Dtos
{
    public class QuestionDto
    {
        public string QuestionTitle { get; set; }
        public string QuestionBody { get; set; }
        public string CorrectAnswer { get; set; }
        public string SearchText { get; set; }
        public int SuggestedDifficulty { get; set; }
    }
}