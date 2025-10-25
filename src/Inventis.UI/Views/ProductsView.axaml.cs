using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Inventis.Application.Products.Dtos;
using Inventis.UI.ViewModels;

namespace Inventis.UI.Views;

internal sealed partial class ProductsView : UserControl
{
    public ProductsView()
    {
        InitializeComponent();

		ProductsDataGrid.SelectionChanged += (s, e) =>
		{
			var dg = (DataGrid)s!;
			var vm = (ProductsViewModel)DataContext!;

			vm.SelectedProducts.Clear();

			foreach (var item in dg.SelectedItems.Cast<ProductDto>())
			{
				vm.SelectedProducts.Add(item);
			}
		};
	}

	protected override async void OnLoaded(RoutedEventArgs e)
	{
		if (DataContext is ProductsViewModel vm)
		{
			await vm.OnLoaded(CancellationToken.None);
		}
	}
}
