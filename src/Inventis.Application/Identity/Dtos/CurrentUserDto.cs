namespace Inventis.Application.Identity.Dtos;

/// <summary>
/// Represents a dto for the current user in the application,
/// containing information about the user's login state and attributes.
/// </summary>
public sealed class CurrentUserDto
{
	private CurrentUserDto(
		string userId,
		string username,
		string firstName,
		string lastName)
	{
		UserId = userId;
		Username = username;
		FullName = $"{firstName} {lastName}";
		FirstName = firstName;
		LastName = lastName;
	}

	/// <summary>
	/// Current logged in user identifier.
	/// </summary>
	public string UserId { get; init; }

	/// <summary>
	/// Gets or sets the name of the user.
	/// </summary>
	public string Username { get; init; }

	/// <summary>
	/// Current logged-in user first name.
	/// </summary>
	public string FirstName { get; init; }

	/// <summary>
	/// Current logged-in user last name.
	/// </summary>
	public string LastName { get; }

	/// <summary>
	/// Current logged-in user full name.
	/// </summary>
	public string FullName { get; }

	public static CurrentUserDto Create(
		Ulid userId,
		string username,
		string firstName,
		string lastName)
		=> new(
			userId.ToString(),
			username,
			firstName,
			lastName);
}
	
