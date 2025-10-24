using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventis.UI.Models;

/// <summary>
/// Singleton used to observe current UI state
/// </summary>
public partial class UiState : ObservableObject
{
	[ObservableProperty]
	private bool _isBusy = false;

	/// <summary>
	/// Marks variable <see cref="IsBusy"/> as true.
	/// If variable is true, UI is blocked.
	/// </summary>
	public void MarkAsBusy()
		=> IsBusy = true;

	/// <summary>
	/// Marks variable <see cref="IsBusy"/> as false.
	/// If variable is false, UI is not blocked.
	/// </summary>
	public void MarkAsFree()
		=> IsBusy = false;
}

