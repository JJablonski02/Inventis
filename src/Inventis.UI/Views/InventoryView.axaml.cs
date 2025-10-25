using Avalonia.Controls;
using Avalonia.Interactivity;
using Inventis.UI.ViewModels;

namespace Inventis.UI.Views;

internal sealed partial class InventoryView : UserControl
{
    public InventoryView()
    {
        InitializeComponent();
	}

	protected override async void OnLoaded(RoutedEventArgs e)
	{
		if (this.DataContext is InventoryViewModel viewModel)
		{
			await viewModel.OnLoadedAsync(CancellationToken.None);
		}
	}
}
