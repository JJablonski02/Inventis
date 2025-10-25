using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventis.Application.Exceptions;
using Inventis.Application.Inventories.Services;
using Inventis.Domain.Inventories.Constants;
using Inventis.UI.Handlers;
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

	[ObservableProperty]
	private bool _inventoryOpen = false;

	[ObservableProperty]
	private string? _lastScanned;

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

			var inventory = await inventoryService.GetInventoryAsync(CancellationToken.None);

			Inventory = InventoryModel.Create(inventory);
		}
		catch (Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>(ex.Message);
		}
	}

	[RelayCommand]
	public async Task CloseInventory()
	{
		if (Inventory is null)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>("Brak otwartej inwentaryzacji do zamknięcia.");
			return;
		}

		var incorrectItems = Inventory.Items
			.Where(item => item.ExpectedQuantity != item.Quantity)
			.ToList();

		if (incorrectItems.Count > 0)
		{
			var sb = new System.Text.StringBuilder();
			sb.AppendLine("Nie zgadzają się ilości następujących produktów:");
			sb.AppendLine();

			foreach (var item in incorrectItems)
			{
				sb.AppendLine($"{item.ProductName}: oczekiwana {item.ExpectedQuantity}, aktualna {item.Quantity}");
			}

			sb.AppendLine();
			sb.AppendLine("Czy na pewno chcesz kontynuować?");

			var result = await windowHandler.OpenDialogWindow<MainWindow>(sb.ToString());

			if (result != DialogResult.Yes)
			{
				return;
			}
		}
		try
		{
			await inventoryService.CloseInventoryAsync(CancellationToken.None);

			Inventory = null;
		}
		catch (Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>(ex.Message);
		}
	}

	public async Task AddScannedProductToInventory(string eanCode, CancellationToken cancellationToken)
	{
		try
		{
			await inventoryService.AddScannedProductAsync(eanCode, cancellationToken);
			await RefreshAsync(cancellationToken);
		}
		catch (NotFoundException)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>("Brak produktu o wprowadzonym kodzie EAN.");
		}
		catch (Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>(ex.Message);
		}
	}

	public Task OnLoadedAsync(CancellationToken cancellationToken)
		=> RefreshAsync(cancellationToken);

	private async Task RefreshAsync(CancellationToken cancellationToken)
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

	partial void OnInventoryChanged(InventoryModel? value)
	{
		InventoryOpen = value is not null;
	}
}
