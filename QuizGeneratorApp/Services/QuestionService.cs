using Microsoft.EntityFrameworkCore;
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

    public QuestionDto[] GetPage(string searchText, int suggestedDifficulty, int count, int offset)
    {
        return _dbContext.Questions.FromSql(
            @$"SELECT QuestionId, QuestionTitle, QuestionBody, CorrectAnswer, SearchText, SuggestedDifficulty
               FROM dbo.Questions 
               JOIN FREETEXTTABLE(dbo.Questions, SearchText, {searchText}) FT ON dbo.Questions.QuestionId = FT.[Key] 
               WHERE SuggestedDifficulty = {suggestedDifficulty}
               ORDER BY FT.RANK DESC
               OFFSET {(offset - 1) * count} ROWS FETCH NEXT {count} ROWS ONLY")
               .Select(q => new QuestionDto
               {
                   QuestionId = q.QuestionId,
                   QuestionTitle = q.QuestionTitle,
                   QuestionBody = q.QuestionBody,
                   CorrectAnswer = q.CorrectAnswer,
                   SearchText = q.SearchText,
                   SuggestedDifficulty = q.SuggestedDifficulty
               }).ToArray();
    }

    public void Save(QuestionDto[] questionsDto)
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
        SearchText = questionDto.SearchText,
        SuggestedDifficulty = questionDto.SuggestedDifficulty
    };
}
