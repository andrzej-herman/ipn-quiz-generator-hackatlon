using QuizGeneratorApp.Models;

namespace QuizGeneratorApp.Services;

public interface IQuestionService
{
    public void Save(QuestionDto[] questionsDto);
    public QuestionDto[] GetPage(string searchText, int count, int offset);
}