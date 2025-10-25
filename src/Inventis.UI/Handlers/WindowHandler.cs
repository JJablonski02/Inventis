using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.ViewModels;
using Inventis.UI.Views;

namespace Inventis.UI.Handlers;

internal sealed class WindowHandler : IWindowHandler
{
	public Task OpenDialogErrorWindow<TOwner>(string message) where TOwner : Window
	{
		var owner = GetOpenedWindow<TOwner>();
		var errorWindow = new ErrorWindow(message);

		return errorWindow.ShowDialog(owner);
	}

	public void CloseWindow<TWindow>() where TWindow : Window
	{
		var window = GetOpenedWindow<TWindow>();
		window.Close();
	}

	public Task OpenWindowAsDialog<TWindow, TOwner>(ViewModelBase viewModel, object? additionalParameters = null) where TWindow : Window, new() where TOwner : Window
	{
		var owner = GetOpenedWindow<TOwner>();
		CheckIsWindowOpened<TWindow>();

		viewModel.SetAdditionalParameters(additionalParameters);

		var window = new TWindow { DataContext = viewModel };

		return window.ShowDialog(owner);
	}

	public async Task<DialogResult> OpenDialogWindow<TOwner>(string message, int? height = null) where TOwner : Window
	{
		var owner = GetOpenedWindow<TOwner>();

		var dialogWindow = new DialogWindow(message);
		dialogWindow.SetHeight(height);
		var result = await dialogWindow.ShowDialog<DialogResult>(owner);
		dialogWindow.ResetHeight();

		return result;
	}

	public async Task<TResult?> OpenWindowAsDialogWithResult<TWindow, TOwner, TResult>(ViewModelBase viewModel, object? additionalParameters = null)
		where TWindow : Window, new() where TOwner : Window
	{
		var owner = GetOpenedWindow<TOwner>();
		CheckIsWindowOpened<TWindow>();

		viewModel.SetAdditionalParameters(additionalParameters);

		var window = new TWindow { DataContext = viewModel };
		return await window.ShowDialog<TResult>(owner);
	}

	private static Window GetOpenedWindow<TWindow>() where TWindow : Window
	{
		if (Avalonia.Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime appLifetime)
		{
			throw new ArgumentException($"Unable to find window of type: {typeof(TWindow)}");
		}

		var window = appLifetime.Windows.SingleOrDefault(x => x is TWindow);

		if (window is null)
		{
			throw new ArgumentException($"Unable to find window of type: {typeof(TWindow)}");
		}

		return window;
	}

	private static void CheckIsWindowOpened<TWindow>() where TWindow : Window
	{
		if (Avalonia.Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime appLifetime)
		{
			throw new ArgumentException($"Unable to find window of type: {typeof(TWindow)}");
		}

		var isWindowOpened = appLifetime.Windows.Any(x => x is TWindow);

		if (isWindowOpened)
		{
			throw new ArgumentException($"Unable to find window of type: {typeof(TWindow)}");
		}
	}
}
