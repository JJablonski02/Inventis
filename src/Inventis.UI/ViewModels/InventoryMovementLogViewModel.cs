using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventis.Application.Products.Services;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.Models.Products;
using Inventis.UI.Views;

namespace Inventis.UI.ViewModels;

internal sealed partial class InventoryMovementLogViewModel(
	IProductService productService,
	IWindowHandler windowHandler) : ViewModelBase
{
	public ObservableCollection<ProductInventoryMovementLogModel> Logs { get; } = [];

	[ObservableProperty]
	private string? _productName;

	public async Task OnLoaded()
	{
		if (AdditionalParameters is not Ulid productId)
		{
			return;
		}

		try
		{
			var product = await productService.GetByIdAsync(productId, CancellationToken.None);

			ProductName = $"Aktywność produktu: {product.Name}";

			Logs.Clear();

			foreach (var log in product.Logs)
			{
				Logs.Add(ProductInventoryMovementLogModel.FromDto(log));
			}
		}
		catch (Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<InventoryMovementLogWindow>(ex.Message);
		}
	}

	[RelayCommand]
	public void CloseWindow()
		=> windowHandler.CloseWindow<InventoryMovementLogWindow>();
}
