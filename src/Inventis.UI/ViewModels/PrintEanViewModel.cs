using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventis.Domain.Products;
using Inventis.UI.Handlers;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.Models.PrintEans;
using Inventis.UI.Services;
using Inventis.UI.Views;

namespace Inventis.UI.ViewModels;

internal sealed partial class PrintEanViewModel : ViewModelBase
{
	private readonly IWindowHandler _windowHandler;
	private readonly IPrinterService _printerService;

	public PrintEanViewModel(
		IWindowHandler windowHandler,
		IPrinterService printerService)
	{
		_windowHandler = windowHandler;
		_printerService = printerService;

		Products.CollectionChanged += (s, e) =>
		{
			if (e.NewItems is null)
			{
				return;
			}

			foreach (var item in e.NewItems)
			{
				if (item is PrintEanModel product)
				{
					product.PropertyChanged += (sender, args) =>
					{
						if (args.PropertyName == nameof(PrintEanModel.LabelCount))
						{
							OnPropertyChanged(nameof(TotalLabels));
						}
					};
				}
			}
		};
	}

	private ObservableCollection<PrintEanModel> _products = [];
	public ObservableCollection<PrintEanModel> Products
	{
		get => _products;
		set
		{
			_products = value;
			OnPropertyChanged(nameof(Products));
			OnPropertyChanged(nameof(TotalLabels));
		}
	}

	private ObservableCollection<PrintEanModel> _selectedProducts = [];
	public ObservableCollection<PrintEanModel> SelectedProducts
	{
		get => _selectedProducts;
		set
		{
			_selectedProducts = value;
			OnPropertyChanged(nameof(SelectedProducts));
			OnPropertyChanged(nameof(TotalLabels));
		}
	}

	[ObservableProperty]
	private bool _isPrintAll = true;

	[ObservableProperty]
	private bool _isPrintSelected;

	partial void OnIsPrintAllChanged(bool value)
	{
		if (value)
		{
			IsPrintSelected = false;
			OnPropertyChanged(nameof(TotalLabels));
		}
	}

	partial void OnIsPrintSelectedChanged(bool value)
	{
		if (value)
		{
			IsPrintAll = false;
			OnPropertyChanged(nameof(TotalLabels));
		}
	}

	[ObservableProperty]
	private bool _printFromShop = true;
	partial void OnPrintFromShopChanged(bool value) => UpdateAllLabelCounts();

	[ObservableProperty]
	private bool _printFromBackroom = true;
	partial void OnPrintFromBackroomChanged(bool value) => UpdateAllLabelCounts();

	[ObservableProperty]
	private bool _printFromWarehouse = true;
	partial void OnPrintFromWarehouseChanged(bool value) => UpdateAllLabelCounts();

	public int TotalLabels => IsPrintSelected ? SelectedProducts.Sum(p => p.LabelCount) : Products.Sum(p => p.LabelCount);

	private void UpdateAllLabelCounts()
	{
		foreach (var product in Products)
		{
			int count = 0;
			if (PrintFromShop)
				count += (int)Math.Round(product.QuantityInStore);
			if (PrintFromBackroom)
				count += (int)Math.Round(product.QuantityInBackroom);
			if (PrintFromWarehouse)
				count += (int)Math.Round(product.QuantityInWarehouse);

			product.LabelCount = count;
		}

		foreach (var product in SelectedProducts)
		{
			int count = 0;
			if (PrintFromShop)
				count += (int)Math.Round(product.QuantityInStore);
			if (PrintFromBackroom)
				count += (int)Math.Round(product.QuantityInBackroom);
			if (PrintFromWarehouse)
				count += (int)Math.Round(product.QuantityInWarehouse);

			product.LabelCount = count;
		}

		OnPropertyChanged(nameof(TotalLabels));
	}

	[RelayCommand]
	public async Task PrintEanCodes()
	{
		if (IsPrintAll && IsPrintSelected)
		{
			await _windowHandler.OpenDialogErrorWindow<PrintEanWindow>("Wybierz jedną z opcji: Drukuj wszystkie lub Drukuj wybrane.");
			return;
		}

		List<string> barCodes = [];
		List<string> selectedEansText = [];

		if (IsPrintSelected)
		{
			selectedEansText = [.. SelectedProducts.Select(p => $"{p.EanCode} x {p.LabelCount}")];

			barCodes = [.. SelectedProducts.SelectMany(p => Enumerable.Repeat(p.EanCode, p.LabelCount))];
		}
		else if (IsPrintAll)
		{
			selectedEansText = [.. Products.Select(p => $"{p.EanCode} x {p.LabelCount}")];

			barCodes = [.. Products.SelectMany(p => Enumerable.Repeat(p.EanCode, p.LabelCount))];
		}

		if (barCodes.Count is 0)
		{
			await _windowHandler.OpenDialogErrorWindow<PrintEanWindow>("Brak wybranych kodów EAN do wydruku.");
			return;
		}

		var message = $"Czy na pewno chcesz wydrukować {barCodes.Count} etykiet?\n" +
					  string.Join("\n", selectedEansText);

		var result = await _windowHandler.OpenDialogWindow<PrintEanWindow>(message);

		if (result != DialogResult.Yes)
		{
			return;
		}

		_printerService.PrintBarcodes(barCodes);

		CloseWindow();
	}

	[RelayCommand]
	public void CloseWindow()
		=> _windowHandler.CloseWindow<PrintEanWindow>();

	public void OnLoaded()
	{
		if (AdditionalParameters is IEnumerable<PrintEanModel> printEanModels)
		{
			foreach (var printEan in printEanModels)
			{
				Products.Add(printEan);

				if (printEan.IsSelected)
				{
					SelectedProducts.Add(printEan);
				}
			}
		}
	}
}
