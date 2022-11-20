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
    public QuestionDto[] GetPage([FromQuery] string searchText, [FromQuery] int suggestedDifficulty, 
        [FromQuery] int count, [FromQuery] int offset)
    {
        return _questionService.GetPage(searchText, suggestedDifficulty, count, offset);
    }

    [HttpPost("save")]
	public void Save([FromBody] QuestionDto[] questionDtos)
	{
		_questionService.Save(questionDtos);
	}

    [HttpPost("generate")]
    public byte[] GeneratePdf([FromBody] QuestionDto[] questionDtos)
    {
        return _quizGeneratorService.Generate(questionDtos);
    }
}