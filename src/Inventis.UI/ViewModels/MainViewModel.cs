using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventis.UI.Controls;
using Inventis.UI.Handlers;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.Models;
using Inventis.UI.Router;
using Inventis.UI.Services;
using Inventis.UI.Views;

namespace Inventis.UI.ViewModels;

internal sealed partial class MainViewModel : ViewModelBase
{
	private readonly IWindowHandler _windowHandler;
	private readonly ISessionService _sessionService;

	public MainViewModel(
		IHistoryRouter<ViewModelBase> historyRouter,
		ISessionService sessionService,
		IWindowHandler windowHandler)
	{
		// Registers the event for the current ViewModel change in the router,
		// to set the content to the new ViewModel when the route changes.
		historyRouter.CurrentViewModelChanged += viewModel => Content = viewModel;
		InitializeTimer();
		_windowHandler = windowHandler;
		_sessionService = sessionService;
		_sessionService.PropertyChanged += SessionService_PropertyChanged;
		historyRouter.GoTo<LoginViewModel>();
	}

	/// <summary>
	/// Variable to store the current ViewModel that is displayed in the view.
	/// </summary>
	[ObservableProperty]
	private ViewModelBase _content = null!;

	/// <summary>
	/// Title bar model
	/// </summary>
	public TitleBarModel TitleBar { get; } = new();

	/// <summary>
	/// Timer reference
	/// </summary>
	private DispatcherTimer? _timer;

	private void InitializeTimer()
	{
		_timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
		_timer.Tick += (_, _) => UpdateTime();
		_timer.Start();
	}

	/// <summary>
	/// Update the current displaying time
	/// </summary>
	private void UpdateTime()
	{
		TitleBar.CurrentTime = DateTime.Now.ToString("HH:mm ddd d MMMM yyyy");
	}

	[RelayCommand]
	public async Task CloseWindow()
	{
		var result = await _windowHandler.OpenDialogWindow<MainWindow>("Czy chcesz zamknąć program?");
		if (result == DialogResult.Yes && Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktop)
		{
			classicDesktop.Shutdown();
		}
	}

	[RelayCommand]
	private void MinifyWindow(object sender)
	{
		if ((sender as WindowButton)?.GetVisualRoot() is Window window)
		{
			window.WindowState = WindowState.Minimized;
		}
	}

	/// <summary>
	/// Updates state based on the provided CurrentUser model.
	/// </summary>
	/// <param name="sender">Sender object</param>
	/// <param name="e">Event</param>
	private void SessionService_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(_sessionService.CurrentUser))
		{
			TitleBar.CurrentUser = _sessionService.CurrentUser;
			TitleBar.IsLoggedIn = _sessionService.CurrentUser is not null;
		}
	}
}
