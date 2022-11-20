namespace QuizGeneratorApp.Database.DbTools
{
	public static class ConnectionStringCreator
	{
		public static string BuildConnectionString(string? server, string? db, string? user, string? password)
		{
			return $"Server={server};Initial Catalog={db};Persist Security Info=False;User ID={user};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
		}
	}
}

