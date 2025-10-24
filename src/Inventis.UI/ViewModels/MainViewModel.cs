using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Inventis.UI.Models;
using Inventis.UI.Router;

namespace Inventis.UI.ViewModels;

internal sealed partial class MainViewModel : ViewModelBase
{
	public MainViewModel(
		IHistoryRouter<ViewModelBase> historyRouter,
		UiState uiState)
	{
		// Registers the event for the current ViewModel change in the router,
		// to set the content to the new ViewModel when the route changes.
		historyRouter.CurrentViewModelChanged += viewModel => Content = viewModel;
		InitializeTimer();
		UiState = uiState;
		historyRouter.GoTo<LoginViewModel>();
	}

	/// <summary>
	/// Current UI state
	/// </summary>
	[ObservableProperty]
	private UiState _uiState;

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
}
