using Avalonia.Controls;
using Inventis.UI.ViewModels;

namespace Inventis.UI.Handlers.Interfaces;

internal interface IWindowHandler
{
	/// <summary>
	/// Closes window of specific Type.
	/// </summary>
	/// <typeparam name="TWindow">Type of window to be closed</typeparam>
	void CloseWindow<TWindow>() where TWindow : Window;

	/// <summary>
	/// Opens error window as a dialog
	/// </summary>
	/// <typeparam name="TOwner">Owner of dialog window</typeparam>
	/// <param name="message">Text message for error window</param>
	Task OpenDialogErrorWindow<TOwner>(string message) where TOwner : Window;


	/// <summary>
	/// Opens window as a dialog with provided ViewModel.
	/// </summary>
	/// <typeparam name="TWindow">Type of 'Window' to be opened</typeparam>
	/// <typeparam name="TOwner">Owner of dialog window</typeparam>
	/// <param name="viewModel">ViewModel to be opened as DataContext</param>
	/// <param name="additionalParameters"> Optional additional parameters to be passed to the ViewModel. These parameters can be used
	Task OpenWindowAsDialog<TWindow, TOwner>(ViewModelBase viewModel, object? additionalParameters = null) where TWindow : Window, new() where TOwner : Window;

	/// <summary>
	/// Opens dialog window
	/// </summary>
	/// <typeparam name="TOwner">Owner of dialog window</typeparam>
	/// <param name="message">Text message for dialog window</param>
	/// <param name="height">Height for dialog window</param>
	/// <returns>Result from the dialog window. For more, see <see cref="DialogResult"/></returns>
	Task<DialogResult> OpenDialogWindow<TOwner>(string message, int? height = null) where TOwner : Window;

	/// <summary>
	/// Opens window as dialog with provided ViewModel and returned dialog result
	/// </summary>
	/// <typeparam name="TWindow">Type of 'Window' to be opened</typeparam>
	/// <typeparam name="TOwner">Owner of dialog window</typeparam>
	/// <typeparam name="TResult">Type of result to be returned</typeparam>
	/// <param name="viewModel">The ViewModel to be used by the window. </param>
	/// <param name="additionalParameters"> Optional additional parameters to be passed to the ViewModel. These parameters can be used
	/// to customize the behavior of the ViewModel or the window itself.</param>
	/// <returns>A Task that represents the asynchronous operation, with a TResult result returned after the dialog is closed.</returns>
	Task<TResult?> OpenWindowAsDialogWithResult<TWindow, TOwner, TResult>(ViewModelBase viewModel, object? additionalParameters = null)
		where TWindow : Window, new() where TOwner : Window;
}
