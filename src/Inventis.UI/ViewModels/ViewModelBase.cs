using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventis.UI.ViewModels;

/// <summary>
/// Base ViewModel class inheriting from ObservableObject
/// </summary>
internal abstract partial class ViewModelBase : ObservableObject
{
	/// <summary>
	/// This property is used to store any extra data that may be passed to the ViewModel.
	/// </summary>
	[ObservableProperty]
	private object? _additionalParameters;

	/// <summary>
	/// Sets the additional parameters for the ViewModel.
	/// This method can be used to assign any extra data to the ViewModel, which may be needed for its operation.
	/// </summary>
	/// <param name="additionalParameters">The additional parameters to be assigned to the ViewModel.</param>
	public void SetAdditionalParameters(object? additionalParameters)
	{
		AdditionalParameters = additionalParameters;
	}

	partial void OnAdditionalParametersChanged(object? value)
	{
		OnAdditionalParametersChangedCore(value);
	}

	protected virtual void OnAdditionalParametersChangedCore(object? value)
	{
	}
}
