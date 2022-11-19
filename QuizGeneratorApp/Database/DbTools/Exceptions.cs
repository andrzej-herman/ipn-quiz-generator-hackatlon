namespace QuizGeneratorApp.Database.DbTools;

public class DuplictedEmailException : Exception
{
	public DuplictedEmailException() : base("Podany adres email jest już zarejestrowany. Prosze wybrać inny lub zalogować się.") {}
}

public class InvalidLoginAttempt : Exception
{
	public InvalidLoginAttempt() : base("Nieprawidowy adres email lub/i hasło.") {}
}