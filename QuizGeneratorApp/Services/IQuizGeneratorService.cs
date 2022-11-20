using QuizGeneratorApp.Models;

namespace QuizGeneratorApp.Services;

public interface IQuizGeneratorService
{
    public byte[] Generate(QuestionDto[] questionDtos);
}