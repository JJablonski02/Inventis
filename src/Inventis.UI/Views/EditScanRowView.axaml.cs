using Avalonia.Controls;
using Avalonia.Interactivity;
using Inventis.UI.ViewModels;

namespace Inventis.UI.Views;

internal sealed partial class EditScanRowView : UserControl
{
	public EditScanRowView()
	{
		InitializeComponent();
	}

	protected override async void OnLoaded(RoutedEventArgs e)
	{
		if (DataContext is not EditScanRowViewModel vm)
		{
			return;
		}

		await vm.OnLoaded();
	}
}
