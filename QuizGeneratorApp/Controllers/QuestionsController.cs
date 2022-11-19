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

    public QuestionsController(IQuestionService questionService)
	{
		_questionService = questionService;
	}

    [HttpGet]
    public QuestionDto[] GetPage([FromQuery] string searchText, [FromQuery] int count, [FromQuery] int offset)
    {
        return _questionService.GetPage(searchText, count, offset);
    }

    [HttpPost("save")]
	public void Save([FromBody] QuestionDto[] questionDtos)
	{
		_questionService.Save(questionDtos);
	}
}