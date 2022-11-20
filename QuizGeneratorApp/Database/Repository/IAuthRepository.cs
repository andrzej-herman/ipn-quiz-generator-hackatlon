using QuizGeneratorApp.Database.Models;

namespace QuizGeneratorApp.Database.Repository
{
	public interface IAuthRepository
	{
		Task<QuizUser> Register(string? email, string? firstName, string? lastName, string? password);
		Task<QuizUser> Login(string? email, string? password);
	}
}

