

using Microsoft.EntityFrameworkCore;
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
	
	public async Task<QuizUser> Register(string? email, string? firstName, string? lastName, string? password)
	{
		var user = _context.QuizUsers.FirstOrDefault(u => u.QuizUserEmail.ToLower() == email!.ToLower());
		if (user != null) throw new DuplictedEmailException();
		DbEncryption.CreateHashAndSalt(password, out var hash, out var salt);
		user = new QuizUser
		{
			QuizUserId = Guid.NewGuid().ToString(),
			QuizUserEmail = email!.ToLower().Trim(),
			QuizUserFirstName = firstName!.Trim(),
			QuizUserLastName = lastName!.Trim(),
			PasswordHash = hash,
			PasswordSalt = salt
		};
		
		await _context.QuizUsers.AddAsync(user);
		await _context.SaveChangesAsync();
		return user;
	}
	
	public async Task<QuizUser> Login(string? email, string? password)
	{
		var user = await _context.QuizUsers.FirstOrDefaultAsync(u => u.QuizUserEmail.ToLower() == email!.ToLower());
		if (user == null) throw new InvalidLoginAttempt();
		if (!DbEncryption.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
			throw new InvalidLoginAttempt();

		return user;
	}
}