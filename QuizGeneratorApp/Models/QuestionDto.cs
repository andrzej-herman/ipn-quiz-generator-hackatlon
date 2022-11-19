namespace QuizGeneratorApp.Models;

public class QuestionDto
{
    public int QuestionId { get; set; }
    public string QuestionTitle { get; set; } = null!;
    public string QuestionBody { get; set; } = null!;
    public string CorrectAnswer { get; set; } = null!;
    public string SearchText { get; set; } = null!;
}