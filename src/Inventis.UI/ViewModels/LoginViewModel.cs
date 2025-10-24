using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Input;
using Inventis.UI.Router;
using Inventis.UI.Services;

namespace Inventis.UI.ViewModels;

/// <summary>
/// Partial class for the 'View-Model' of the LoginView.
/// </summary>
internal sealed partial class LoginViewModel(
    ISessionService sessionService,
    IHistoryRouter<ViewModelBase> router) : ViewModelBase
{

	private bool _confirmEnabled;

	/// <summary>
	/// Flag that determines if submit button is enabled.
	/// </summary>
	public bool ConfirmEnabled
	{
		get => _confirmEnabled;
		private set => SetProperty(ref _confirmEnabled, value);
	}

    private string? _login;

    /// <summary>
    /// Login
    /// </summary>
    [Required]
    public string? Login
    {
        get => _login;
        set
        {
            SetProperty(ref _login, value);
			UpdateConfirmEnabled();
		}
    }

    private string? _password;

    /// <summary>
    /// Password.
    /// </summary>
    [Required]
    public string? Password
    {
        get => _password;
        set
        {
            SetProperty(ref _password, value);
            UpdateConfirmEnabled();

		}
    }

	private void UpdateConfirmEnabled()
	{
		ConfirmEnabled = !string.IsNullOrWhiteSpace(_login) && !string.IsNullOrWhiteSpace(_password);
	}

    [RelayCommand]
	public async Task HandleLogin()
    {
        var loginSucceeded = await sessionService.LoginAsync(Login!, Password!, CancellationToken.None);

        if (!loginSucceeded)
        {
            return;
        }

        router.GoTo<HomeViewModel>();
    }
}
