using System.ComponentModel;

namespace Inventis.UI.Models;

/// <summary>
/// Represents the model for the title bar in the application.
/// </summary>
public sealed class TitleBarModel : INotifyPropertyChanged
{
	// Holds the current user's information
	private CurrentUser? _currentUser;

	// Holds the current time as a string
	private string? _currentTime;

	// Indicates whether the user is logged in
	private bool _isLoggedIn;

	// Holds the inventis version
	private string? _inventisVersion;


	/// <summary>
	/// Gets or sets the current user.
	/// Notifies subscribers when the current user changes.
	/// </summary>
	public CurrentUser? CurrentUser
	{
		get => _currentUser;
		set
		{
			if (_currentUser != value)
			{
				_currentUser = value;
				OnPropertyChanged(nameof(CurrentUser));
			}
		}
	}

	/// <summary>
	/// Gets or sets the current time.
	/// Notifies subscribers when the current time changes.
	/// </summary>
	public string? CurrentTime
	{
		get => _currentTime;
		set
		{
			if (_currentTime != value)
			{
				_currentTime = value;
				OnPropertyChanged(nameof(CurrentTime));
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether the user is logged in.
	/// Notifies subscribers when the login status changes.
	/// </summary>
	public bool IsLoggedIn
	{
		get => _isLoggedIn;
		set
		{
			if (_isLoggedIn != value)
			{
				_isLoggedIn = value;
				OnPropertyChanged(nameof(IsLoggedIn));
			}
		}
	}

	/// <summary>
	/// Gets or sets the cashdesk version
	/// </summary>
	public string? CashdeskVersion
	{
		get => _inventisVersion;
		set
		{
			if (_inventisVersion != value)
			{
				_inventisVersion = value;
				OnPropertyChanged(nameof(CashdeskVersion));
			}
		}
	}

	// Event to notify subscribers of property changes
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Raises the PropertyChanged event for the specified property.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed.</param>
	private void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
