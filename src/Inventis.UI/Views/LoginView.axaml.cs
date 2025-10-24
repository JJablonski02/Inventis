using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Inventis.UI.ViewModels;

namespace Inventis.UI.Views;

/// <summary>
/// Class that initializes the 'Login' view component.
/// </summary>
internal sealed partial class LoginView : UserControl
{
    /// <summary>
    /// Reference to this view model
    /// </summary>
    private LoginViewModel? _loginViewModel;

    /// <summary>
    /// Constructor for the LoginView class.
    /// Initializes the components of the Login view.
    /// </summary>
    public LoginView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles text input key down event
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event</param>
    public void HandleKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _loginViewModel?.HandleLogin();
        }
    }

    /// <summary>
    /// Invokes logic after UI loaded
    /// </summary>
    /// <param name="e">Event</param>
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        _loginViewModel = (LoginViewModel?)DataContext;
    }
}
