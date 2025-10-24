using Avalonia.Controls;
using Inventis.UI.ViewModels;

namespace Inventis.UI.Handlers.Interfaces;

internal interface IWindowHandler
{
	/// <summary>
	/// Opens error window as a dialog
	/// </summary>
	/// <typeparam name="TOwner">Owner of dialog window</typeparam>
	/// <param name="message">Text message for error window</param>
	Task OpenDialogErrorWindow<TOwner>(string message) where TOwner : Window;

	/// <summary>
	/// Opens window as a dialog
	/// </summary>
	/// <typeparam name="TWindow"></typeparam>
	/// <typeparam name="TOwner"></typeparam>
	/// <param name="viewModel"></param>
	/// <param name="additionalParameters"></param>
	/// <returns></returns>
	Task OpenWindowAsDialog<TWindow, TOwner>(ViewModelBase viewModel, object? additionalParameters = null) where TWindow : Window, new() where TOwner : Window;
}
