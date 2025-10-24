using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Inventis.Domain.Products;
using Inventis.UI.Models.Products;
using Inventis.UI.ViewModels;

namespace Inventis.UI.Views;

internal sealed partial class ProductView : UserControl
{
	public ProductView()
	{
		InitializeComponent();
	}

	protected override void OnLoaded(RoutedEventArgs e)
	{
		if (DataContext is not ProductViewModel productViewModel)
		{
			return;
		}

		productViewModel.Product.PropertyChanged += (_, args) =>
		{
			if (args.PropertyName is nameof(ProductModel.EanCode) &&
				productViewModel.Product.EanCode?.Length is 13)
			{
				BarcodePreview.ShowBarcode(productViewModel.Product.EanCode);
			}
		};

		if (productViewModel.Product.EanCode is null)
		{
			productViewModel.GenerateEan();
		}
	}
}
