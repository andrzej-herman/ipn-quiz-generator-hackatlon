using QuizGeneratorApp.Models;

namespace QuizGeneratorApp.Services;

public interface IQuizGeneratorService
{
    public Task<byte[]> GenerateAsync(QuestionDto[] questionDtos);
}