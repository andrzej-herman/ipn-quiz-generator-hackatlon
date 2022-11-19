using QuizGeneratorApp.Database.BaseContext;
using QuizGeneratorApp.Database.Models;
using QuizGeneratorApp.Models;

namespace QuizGeneratorApp.Services;

public class QuestionService : IQuestionService
{
    private readonly IpnQuizContext _dbContext;

    public QuestionService(IpnQuizContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void SaveQuestions(QuestionDto[] questionsDto)
    {
        _dbContext.AddRange(questionsDto.Select(MapDtoToDbModel));
        _dbContext.SaveChanges();
    }

    private Question MapDtoToDbModel(QuestionDto questionDto) => new Question
    {
        QuestionId = questionDto.QuestionId,
        QuestionBody = questionDto.QuestionBody,
        QuestionTitle = questionDto.QuestionTitle,
        CorrectAnswer = questionDto.CorrectAnswer,
        SearchText = questionDto.SearchText
    };
}
