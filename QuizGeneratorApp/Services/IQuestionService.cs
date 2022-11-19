using QuizGeneratorApp.Models;

namespace QuizGeneratorApp.Services;

public interface IQuestionService
{
    public void SaveQuestions(QuestionDto[] questionsDto);
}