namespace QuizGeneratorCommon.Auth;

public class AuthResponse
{
	public bool Result { get; set; }
	public UserSession UserSession { get; set; }
	public string? Error { get; set; }
}