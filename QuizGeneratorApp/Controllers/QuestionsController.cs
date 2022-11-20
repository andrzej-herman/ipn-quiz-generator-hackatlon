using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuizGeneratorApp.Models;
using QuizGeneratorApp.Services;
using System.ComponentModel.DataAnnotations;

namespace QuizGeneratorApp.Controllers;

[Route("api/[controller]")]
public class QuestionsController
{
    private IQuestionService _questionService;
    private IQuizGeneratorService _quizGeneratorService;

    public QuestionsController(IQuestionService questionService, IQuizGeneratorService quizGeneratorService)
	{
		_questionService = questionService;
        _quizGeneratorService = quizGeneratorService;
    }

    [HttpGet]
    public async Task<QuestionDto[]> GetPageAsync([FromQuery] string searchText, [FromQuery] int suggestedDifficulty, 
        [FromQuery] int count, [FromQuery] int offset)
    {
        return await _questionService.GetPageAsync(searchText, suggestedDifficulty, count, offset);
    }

    [HttpPost("save")]
	public void Save([FromBody] QuestionDto[] questionDtos)
	{
		_questionService.Save(questionDtos);
	}

    [HttpPost("generate")]
    public async Task<byte[]> GeneratePdf([FromBody] QuestionDto[] questionDtos)
    {
        return await _quizGeneratorService.GenerateAsync(questionDtos);
    }
}