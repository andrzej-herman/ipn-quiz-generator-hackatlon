using System.Security.Cryptography;
using System.Text;

namespace QuizGeneratorApp.Database.DbTools;

public static class DbEncryption
{
	public static void CreateHashAndSalt(string? password, out byte[] hash, out byte[] salt)
	{
		using var hmac = new HMACSHA512();
		salt = hmac.Key;
		hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password?.Trim()!));
	}
	
	public static bool VerifyPassword(string? password, IEnumerable<byte> hash, byte[] salt)
	{
		using var hmac = new HMACSHA512(salt);
		if (password == null) return false;
		var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password.Trim()));
		return computedHash.SequenceEqual(hash);
	}
}