

using QuizGeneratorApp.Database.BaseContext;
using QuizGeneratorApp.Database.DbTools;
using QuizGeneratorApp.Database.Models;

namespace QuizGeneratorApp.Database.Repository;

public class AuthRepository : IAuthRepository
{
	private readonly IpnQuizContext _context;
	
	public AuthRepository(IpnQuizContext context)
	{
		_context = context;
	}
	
	public async Task<QuizUser> Register(string? email, string? firstName, string? lastName, string? pasword)
	{
		var user = _context.QuizUsers.FirstOrDefault(u =>
			string.Equals(u.QuizUserEmail, email, StringComparison.CurrentCultureIgnoreCase));

		if (user != null) throw new DuplictedEmailException();
		user = new QuizUser
		{
			QuizUserId = Guid.NewGuid().ToString(),
			QuizUserEmail = email!.ToLower().Trim(),
			QuizUserFirstName = firstName!.Trim(),
			QuizUserLastName = lastName!.Trim()
		};
		
		await _context.QuizUsers.AddAsync(user);
		await _context.SaveChangesAsync();
		return user;
	}
	
	public Task<QuizUser> Login(string? email, string? password)
	{
		return null;
	}
}