using QuizGeneratorApp.Models;

namespace QuizGeneratorApp.Services;

public interface IQuestionService
{
    public void Save(QuestionDto[] questionsDto);
    public Task <QuestionDto[]> GetPageAsync(string searchText, int suggestedDifficulty, int count, int offset);
}