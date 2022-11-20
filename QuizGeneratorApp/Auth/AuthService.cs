using QuizGeneratorApp.Database.Repository;
using QuizGeneratorCommon.Auth;

namespace QuizGeneratorApp.Auth;

public class AuthService : IAuthService
{
	private readonly IAuthRepository _repo;

	public AuthService(IAuthRepository repo)
	{
		_repo = repo;
	}
	
	public async Task<AuthResponse> Register(RegisterRequest request)
	{
		try
		{
			var user = await _repo.Register(request.Email, request.FirstName, request.LastName, request.Password);
			return new AuthResponse() { Result = true, UserSession = new UserSession
			{
				Id = user.QuizUserId,
				UserName = $"{user.QuizUserFirstName} {user.QuizUserLastName}",
				Email = user.QuizUserEmail
			}};
		}
		catch (Exception e)
		{
			return new AuthResponse() { Result = false, Error = e.Message };
		}
	}

	public async Task<AuthResponse> Login(LoginRequest request)
	{
		try
		{
			var user = await _repo.Login(request.Email, request.Password);
			return new AuthResponse() { Result = true, UserSession = new UserSession
			{
				Id = user.QuizUserId,
				UserName = $"{user.QuizUserFirstName} {user.QuizUserLastName}",
				Email = user.QuizUserEmail
			}};
		}
		catch (Exception e)
		{
			return new AuthResponse() { Result = false, Error = e.Message };
		}
	}
}