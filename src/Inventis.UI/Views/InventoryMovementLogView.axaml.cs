using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Inventis.UI.ViewModels;

namespace Inventis.UI.Views;

internal sealed partial class InventoryMovementLogView : UserControl
{
	public InventoryMovementLogView()
	{
		InitializeComponent();
	}

	protected override async void OnLoaded(RoutedEventArgs e)
	{
		if (DataContext is not InventoryMovementLogViewModel inventoryMovementLogViewModel)
		{
			return;
		}

		await inventoryMovementLogViewModel.OnLoaded();
	}
}
