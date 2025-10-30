using System.ComponentModel;
using Inventis.Application.Exceptions;
using Inventis.Application.Identity.Services;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.Models;
using Inventis.UI.Views;

namespace Inventis.UI.Services;

/// <summary>
/// Session service
/// </summary>
internal interface ISessionService
{
	/// <summary>
	/// Event triggered when a property changes, allowing subscribers to react to changes in the service state.
	/// </summary>
	event PropertyChangedEventHandler? PropertyChanged;

	public CurrentUser? CurrentUser { get; }

	Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken);
}

internal sealed class SessionService(
	IIdentityService identityService,
	IWindowHandler windowHandler) : ISessionService
{
	/// <summary>
	/// Event triggered when a property changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	private CurrentUser? _currentUser;

	/// <summary>
	/// Property representing the currently logged-in user.
	/// Returns null if no user is logged in.
	/// </summary>
	public CurrentUser? CurrentUser
	{
		get => _currentUser;
		private set
		{
			if (_currentUser == value)
			{
				return;
			}

			_currentUser = value;
			OnPropertyChanged(nameof(CurrentUser));
		}
	}

	public async Task<bool> LoginAsync(
		string username,
		string password,
		CancellationToken cancellationToken)
	{
		try
		{
			var user = await identityService.HandleLoginAsync(username, password, cancellationToken);

			CurrentUser = CurrentUser.Create(
				user.UserId,
				user.Username,
				user.FirstName,
				user.LastName);

			return true;
		}
		catch (Exception ex)
		{
			if (ex is NotFoundException)
			{
				await windowHandler.OpenDialogErrorWindow<MainWindow>("Błędna nazwa użytkownika");
			}

			if (ex is InvalidOperationException)
			{
				await windowHandler.OpenDialogErrorWindow<MainWindow>("Nieprawidłowe hasło użytkownika");
			}

			return false;
		}
	}

	/// <summary>
	/// Raises the PropertyChanged event for the specified property.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed.</param>
	private void OnPropertyChanged(string propertyName)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
