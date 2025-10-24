namespace Inventis.Domain.Identity;

public sealed class User : Entity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	private User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	private User(
		string username,
		string firstName,
		string lastName,
		string password)
	{
		Username = username;
		FirstName = firstName;
		LastName = lastName;
		Password = password;
	}

	public string Username { get; }
	public string FirstName { get; }
	public string LastName { get; }
	public string Password { get; }

	public static User Create(
		string username,
		string firstName,
		string lastName,
		string password)
		=> new(
			username,
			firstName,
			lastName,
			password);
}
