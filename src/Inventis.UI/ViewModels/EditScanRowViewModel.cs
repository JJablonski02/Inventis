using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventis.Application.DailyInventoryReports.Dtos;
using Inventis.Application.DailyInventoryReports.Services;
using Inventis.Application.Products.Dtos;
using Inventis.Application.Products.Services;
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

			if (_scan is null)
			{
				await windowHandler.OpenDialogErrorWindow<EditScanRowWindow>("Brak wybranego skanu do usunięcia");
				return;
			}

			await scanService.DeleteSoftScan(_scan.Id, CancellationToken.None);

			Close();
		}
		catch (Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<EditScanRowWindow>(ex.Message);
		}
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
				new QuantityOption(QuantityType.InStore, $"Ilość w sklepie ({_productDto.CurrentQuantityInStore} szt.)"),
				new QuantityOption(QuantityType.InBackroom, $"Ilość na zapleczu ({_productDto.CurrentQuantityInBackroom} szt.)"),
				new QuantityOption(QuantityType.InWarehouse, $"Ilość w magazynie ({_productDto.CurrentQuantityInWarehouse} szt.)")
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
