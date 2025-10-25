using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventis.Application.DailyInventoryReports.Dtos;
using Inventis.Application.DailyInventoryReports.Services;
using Inventis.Application.Products.Dtos;
using Inventis.Application.Products.Services;
using Inventis.Domain.Products;
using Inventis.Domain.Products.Constants;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.Views;

namespace Inventis.UI.ViewModels;

internal sealed partial class EditScanRowViewModel(
	IWindowHandler windowHandler,
	IProductService productService,
	IDailyInventoryScanService scanService) : ViewModelBase
{
	[ObservableProperty]
	private bool _isDeleteAction = true;

	[ObservableProperty]
	private bool _isCancelAction;

	[ObservableProperty]
	private ObservableCollection<QuantityOption> _stackOptions = [];

	[ObservableProperty]
	private QuantityOption? _selectedStack;

	private ProductDto? _productDto;

	private DailyInventoryScanDto? _scan;

	[RelayCommand]
	private async Task DeleteScan()
	{
		if (_scan is null)
		{
			await windowHandler.OpenDialogErrorWindow<EditScanRowWindow>("Brak wybranego skanu do usunięcia");
			return;
		}

		try
		{
			await scanService.DeleteScan(_scan.Id, CancellationToken.None);
		}
		catch(Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<EditScanRowWindow>(ex.Message);
		}

		Close();
	}

	[RelayCommand]
	private async Task CancelScan()
	{
		try
		{
			if (_productDto is null)
			{
				return;
			}

			if (SelectedStack is null)
			{
				await windowHandler.OpenDialogErrorWindow<EditScanRowWindow>("Brak wybranej ilości, z której zostanie zmniejszony stan.");
				return;
			}

			if (_scan is null)
			{
				await windowHandler.OpenDialogErrorWindow<EditScanRowWindow>("Brak wybranego skanu do usunięcia");
				return;
			}

			var quantity = GetQuantity();

			var result = await windowHandler.OpenDialogWindow<EditScanRowWindow>($"Czy na pewno chcesz zmniejszyć stan magazynowy o jedną sztukę dla produktu '{_productDto.Name}'. " +
				$"Po zmniejszeniu; {quantity}");

			if (result != Handlers.DialogResult.Yes)
			{
				return;
			}

			await scanService.DeleteSoftScan(_scan.Id, CancellationToken.None);

			await productService.DecreaseSingleQuantityAsync(_productDto.Id, SelectedStack.Type, CancellationToken.None);

			Close();
		}
		catch (Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<EditScanRowWindow>(ex.Message);
		}
	}

	private string GetQuantity()
	{
		decimal quantity = SelectedStack!.Type switch
		{
			QuantityType.InStore => _productDto!.QuantityInStore,
			QuantityType.InBackroom => _productDto!.QuantityInBackroom,
			QuantityType.InWarehouse => _productDto!.QuantityInWarehouse,
			_ => throw new InvalidOperationException("Nieznany typ ilości.")
		};

		if (quantity == 0)
		{
			throw new InvalidOperationException("Wybrana ilość wynosi 0 i nie można jej użyć.");
		}

		int displayQuantity = quantity > 0 ? (int)Math.Round(quantity) - 1 : (int)Math.Round(quantity);

		return SelectedStack!.Type switch
		{
			QuantityType.InStore => $"Ilość w sklepie będzie wynosić : {displayQuantity} sztuk",
			QuantityType.InBackroom => $"Ilość na zapleczu będzie wynosić : {displayQuantity} sztuk",
			QuantityType.InWarehouse => $"Ilość w magazynie będzie wynosić : {displayQuantity} sztuk",
			_ => throw new InvalidOperationException("Nieznany typ ilości.")
		};
	}

	partial void OnIsDeleteActionChanged(bool value)
	{
		if (value) IsCancelAction = false;
	}

	partial void OnIsCancelActionChanged(bool value)
	{
		if (value) IsDeleteAction = false;
	}

	[RelayCommand]
	public void Close()
		=> windowHandler.CloseWindow<EditScanRowWindow>();

	public async Task OnLoaded()
	{
		if (AdditionalParameters is not DailyInventoryScanDto scan)
		{
			await windowHandler.OpenDialogErrorWindow<EditScanRowWindow>("Niepoprawny skan przekazany do okna.");
			return;
		}

		_scan = scan;

		try
		{
			_productDto = await productService.GetByIdAsync(scan.ProductId, CancellationToken.None);

			StackOptions =
			[
				new QuantityOption(QuantityType.InStore, $"Ilość w sklepie ({_productDto.QuantityInStore} szt.)"),
				new QuantityOption(QuantityType.InBackroom, $"Ilość na zapleczu ({_productDto.QuantityInBackroom} szt.)"),
				new QuantityOption(QuantityType.InWarehouse, $"Ilość w magazynie ({_productDto.QuantityInWarehouse} szt.)")
			];
		}
		catch (Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<EditScanRowWindow>(ex.Message);
		}
	}
}

internal sealed class QuantityOption
{
	public QuantityType Type { get; set; }
	public string Name { get; set; }

	public QuantityOption(QuantityType type, string name)
	{
		Type = type;
		Name = name;
	}
}
