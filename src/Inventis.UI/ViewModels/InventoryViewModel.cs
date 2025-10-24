using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventis.Application.Exceptions;
using Inventis.Application.Inventories.Services;
using Inventis.Domain.Inventories.Constants;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.Models.Inventories;
using Inventis.UI.Services;
using Inventis.UI.Views;

namespace Inventis.UI.ViewModels;

internal sealed partial class InventoryViewModel(
	IInventoryService inventoryService,
	ISessionService sessionService,
	IWindowHandler windowHandler) : ViewModelBase
{

	[ObservableProperty]
	private InventoryModel? _inventory;

	public ObservableCollection<InventorySelectItem> SelectItems { get; } =
	[
		new("Inwentaryzacja całościowa", InventoryType.Total.Value),
		new("Inwentaryzacja sklep i zaplecze", InventoryType.StoreAndBackroom.Value),
		new("Inwentaryzacja sklep", InventoryType.Store.Value),
		new("Inwentaryzacja zaplecze", InventoryType.Backroom.Value),
		new("Inwentaryzacja magazyn", InventoryType.Warehouse.Value),
	];

	[ObservableProperty]
	private InventorySelectItem? _selectedItem;

	[RelayCommand]
	public async Task OpenInventory()
	{
		if (SelectedItem is null)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>("Wybór inwentaryzacji jest wymagany.");
			return;
		}

		try
		{
			await inventoryService.OpenInventoryAsync(
				InventoryType.Parse(SelectedItem.Value),
				Ulid.Parse(sessionService.CurrentUser!.UserId),
				sessionService.CurrentUser!.FullName,
				CancellationToken.None);
		}
		catch (Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>(ex.Message);
		}

	}

	public async Task OnLoadedAsync(CancellationToken cancellationToken)
	{
		try
		{
			var inventory = await inventoryService.GetInventoryAsync(cancellationToken);

			Inventory = InventoryModel.Create(inventory);
		}
		catch (NotFoundException)
		{
			// If not found, nothing to do is required
		}
	}
}
