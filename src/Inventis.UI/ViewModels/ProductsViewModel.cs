using System.Collections.ObjectModel;
using System.Threading;
using CommunityToolkit.Mvvm.Input;
using Inventis.Application.Products.Dtos;
using Inventis.Application.Products.Services;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.Models.PrintEans;
using Inventis.UI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.UI.ViewModels;

internal sealed partial class ProductsViewModel(
	IServiceProvider serviceProvider,
	IProductService productService,
	IWindowHandler windowHandler) : ViewModelBase
{
	public ObservableCollection<ProductDto> Products { get; } = [];

	public ObservableCollection<ProductDto> SelectedProducts { get; } = [];

	[RelayCommand]
	public async Task AddProduct()
	{
		var service = serviceProvider.CreateScope();

		var viewModel = service.ServiceProvider.GetRequiredService<ProductViewModel>();

		await windowHandler.OpenWindowAsDialog<ProductWindow, MainWindow>(viewModel);

		var products = await productService.GetAllAsync(CancellationToken.None);

		Products.Clear();

		foreach (var product in products)
		{
			Products.Add(product);
		}
	}

	[RelayCommand]
	public async Task OpenPrintingWindow()
	{
		if (SelectedProducts.Count is 0)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>("Wybór conajmniej jednego produktu jest wymagany.");
			return;
		}

		var service = serviceProvider.CreateScope();

		var viewModel = service.ServiceProvider.GetRequiredService<PrintEanViewModel>();

		var products = Products.Select(product =>
		{
			bool isSelected = SelectedProducts.Any(prod => prod.Id == product.Id);

			return PrintEanModel.ToPrintEanModel(product, isSelected);
		});

		await windowHandler.OpenWindowAsDialog<PrintEanWindow, MainWindow>(viewModel, products);
	}

	[RelayCommand]
	public async Task OpenSelectedProduct()
	{
		if (SelectedProducts.Count is 0)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>("Wybór produktu jest wymagany.");
			return;
		}

		if (SelectedProducts.Count > 1)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>("Wybierz dokładnie jeden produkt do edycji.");
			return;
		}

		var service = serviceProvider.CreateScope();

		var viewModel = service.ServiceProvider.GetRequiredService<ProductViewModel>();

		await windowHandler.OpenWindowAsDialog<ProductWindow, MainWindow>(viewModel, SelectedProducts[0]);

		var products = await productService.GetAllAsync(CancellationToken.None);

		Products.Clear();

		foreach (var product in products)
		{
			Products.Add(product);
		}
	}

	[RelayCommand]
	public async Task OpenInventoryMovementLog(object? productId)
	{
		if (productId is not Ulid validProductId)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>("Niepoprawny identyfikator wybranego skanu.");
			return;
		}


		var service = serviceProvider.CreateScope();

		var viewModel = service.ServiceProvider.GetRequiredService<InventoryMovementLogViewModel>();

		await windowHandler.OpenWindowAsDialog<InventoryMovementLogWindow, MainWindow>(viewModel, validProductId);
	}

	public async Task OnLoaded(CancellationToken cancellationToken)
	{
		var products = await productService.GetAllAsync(cancellationToken);

		Products.Clear();

		foreach (var product in products)
		{
			Products.Add(product);
		}
	}
}
