using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventis.Application.Products.Dtos;
using Inventis.Application.Products.Dtos.Requests;
using Inventis.Application.Products.Services;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.Models.Products;
using Inventis.UI.Views;

namespace Inventis.UI.ViewModels;

internal sealed partial class ProductViewModel(
	IProductService productService,
	IWindowHandler windowHandler) : ViewModelBase
{
	[ObservableProperty]
	private bool _isUpdate = false;

	partial void OnIsUpdateChanged(bool value)
	{
		if (IsUpdate)
		{
			SaveButtonText = "Zapisz zmiany i zamknij";
		}
	}

	[ObservableProperty]
	private string _saveButtonText = "Utwórz produkt i zamknij";

	[ObservableProperty]
	private ProductModel _product = new();

	[RelayCommand]
	public async Task SaveAndClose()
	{
		try
		{
			if (IsUpdate)
			{
				var updateProduct = await UpdateProductRequest();

				if (updateProduct is null)
				{
					return;
				}

				await productService.UpdateAsync(updateProduct, CancellationToken.None);
				CloseWindow();

				return;
			}
			var createProduct = await CreateProductRequest();

			if (createProduct is null)
			{
				return;
			}

			await productService.CreateAsync(createProduct, CancellationToken.None);
			CloseWindow();
		}
		catch (InvalidOperationException ex)
		{
			await windowHandler.OpenDialogErrorWindow<ProductWindow>(ex.Message);
		}
	}

	[RelayCommand]
	public async Task SaveAndClear()
	{
		if (IsUpdate)
		{
			await windowHandler.OpenDialogErrorWindow<ProductWindow>("Nie można użyć tej opcji podczas edytowania produktu.");
			return;
		}

		var product = await CreateProductRequest();

		if (product is null)
		{
			return;
		}

		try
		{
			await productService.CreateAsync(product, CancellationToken.None);
		}
		catch (InvalidOperationException ex)
		{
			await windowHandler.OpenDialogErrorWindow<ProductWindow>(ex.Message);
		}

		Product = new()
		{
			EanCode = productService.GenerateEan13()
		};
	}

	[RelayCommand]
	public void GenerateEan()
	{
		Product.EanCode = productService.GenerateEan13();
	}

	private async Task<CreateProductRequestDto?> CreateProductRequest()
	{
		var model = Product;

		string? errorMessage = ValidateRequiredFields(model);

		if (errorMessage is not null)
		{
			await windowHandler.OpenDialogErrorWindow<ProductWindow>(errorMessage);
			return null;
		}

		return new CreateProductRequestDto(
			model.Name!,
			model.Description,
			model.EanCode!,
			model.NetPurchasePriceReal!.Value,
			model.GrossPurchasePriceReal!.Value,
			model.NetSalePriceReal!.Value,
			model.GrossSalePriceReal!.Value,
			model.QuantityInStoreReal!.Value,
			model.QuantityInBackroomReal!.Value,
			model.QuantityInWarehouseReal!.Value,
			model.VatRateReal!.Value,
			model.ProviderName,
			model.ProviderContactDetails);
	}

	private async Task<UpdateProductRequestDto?> UpdateProductRequest()
	{
		var model = Product;

		string? errorMessage = ValidateRequiredFields(model, checkId: true);

		if (errorMessage is not null)
		{
			await windowHandler.OpenDialogErrorWindow<ProductWindow>(errorMessage);
			return null;
		}

		return new UpdateProductRequestDto(
			model.Id!.Value,
			model.Name!,
			model.Description,
			model.EanCode!,
			model.NetPurchasePriceReal!.Value,
			model.GrossPurchasePriceReal!.Value,
			model.NetSalePriceReal!.Value,
			model.GrossSalePriceReal!.Value,
			model.QuantityInStoreReal!.Value,
			model.QuantityInBackroomReal!.Value,
			model.QuantityInWarehouseReal!.Value,
			model.VatRateReal!.Value,
			model.ProviderName,
			model.ProviderContactDetails);
	}

	public void OnLoaded()
	{
		if (AdditionalParameters is ProductDto productDto)
		{
			IsUpdate = true;
			Product.FromDto(productDto);
			return;
		}

		if (Product.EanCode is null)
		{
			GenerateEan();
		}
	}

	[RelayCommand]
	public void CloseWindow()
		=> windowHandler.CloseWindow<ProductWindow>();

	private static string? ValidateRequiredFields(ProductModel model, bool checkId = false)
	{
		if (checkId && !model.Id.HasValue)
		{
			return "Brak identyfikatora produktu";
		}

		switch (model)
		{
			case { Name: null }:
				return "Pole 'Nazwa' jest wymagane";
			case { EanCode: null }:
				return "Pole 'Kod EAN' jest wymagane";
			case { NetPurchasePriceReal: null }:
				return "Pole 'Cena zakupu netto' jest wymagane";
			case { GrossPurchasePriceReal: null }:
				return "Pole 'Cena zakupu brutto' jest wymagane";
			case { NetSalePriceReal: null }:
				return "Pole 'Cena sprzedaży netto' jest wymagane";
			case { GrossSalePriceReal: null }:
				return "Pole 'Cena sprzedaży brutto' jest wymagane";
			case { QuantityInStoreReal: null }:
				return "Pole 'Ilość w sklepie' jest wymagane";
			case { QuantityInBackroomReal: null }:
				return "Pole 'Ilość na zapleczu' jest wymagane";
			case { QuantityInWarehouseReal: null }:
				return "Pole 'Ilość w magazynie' jest wymagane";
			case { VatRateReal: null }:
				return "Pole 'Stawka VAT' jest wymagane";
			default:
				return null;
		}
	}
}
