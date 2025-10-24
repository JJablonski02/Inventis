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
	public CurrentUser? CurrentUser { get; }

	Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken);
}

internal sealed class SessionService(
	IIdentityService identityService,
	IWindowHandler windowHandler) : ISessionService
{
	public CurrentUser? CurrentUser { get; private set; }

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
}
