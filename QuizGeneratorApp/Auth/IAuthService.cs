using QuizGeneratorCommon.Auth;

namespace QuizGeneratorApp.Auth;

public interface IAuthService
{
	Task<AuthResponse> Register(RegisterRequest request);
	Task<AuthResponse> Login(LoginRequest request);
}