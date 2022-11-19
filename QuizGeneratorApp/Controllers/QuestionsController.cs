using Microsoft.AspNetCore.Mvc;
using QuizGeneratorApp.Models;
using QuizGeneratorApp.Services;

namespace QuizGeneratorApp.Controllers;

[Route("api/[controller]")]
public class QuestionsController
{
    private IQuestionService _questionService;

    public QuestionsController(IQuestionService questionService)
	{
		_questionService = questionService;
	}

	[HttpPost("save")]
	public void SaveQuestions([FromBody] QuestionDto[] questionDtos)
	{
		_questionService.SaveQuestions(questionDtos);
	}
}