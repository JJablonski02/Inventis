using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.UI.ViewModels;

internal sealed partial class HomeViewModel : ViewModelBase
{
	[ObservableProperty]
	private object _currentViewModel;

	private const string DefaultColor = "#2E2E2E";
	private const string PrimaryColor = "#FF7700";

	[ObservableProperty]
	private string _productsButtonColor = DefaultColor;
	[ObservableProperty]
	private string _salesButtonColor = PrimaryColor;
	[ObservableProperty]
	private string _inventoryButtonColor = DefaultColor;
	[ObservableProperty]
	private string _reportsButtonColor = DefaultColor;

	public HomeViewModel(IServiceProvider serviceProvider)
	{
		var scope = serviceProvider.CreateScope();

		OpenProductsCommand = new RelayCommand(() =>
		{
			var vm = scope.ServiceProvider.GetRequiredService<ProductsViewModel>();
			if (CurrentViewModel?.GetType() != typeof(ProductsViewModel))
				CurrentViewModel = vm;
		});

		OpenSalesCommand = new RelayCommand(() =>
		{
			var vm = scope.ServiceProvider.GetRequiredService<SalesViewModel>();
			if (CurrentViewModel?.GetType() != typeof(SalesViewModel))
				CurrentViewModel = vm;
		});

		OpenInventoryCommand = new RelayCommand(() =>
		{
			var vm = scope.ServiceProvider.GetRequiredService<InventoryViewModel>();
			if (CurrentViewModel?.GetType() != typeof(InventoryViewModel))
				CurrentViewModel = vm;
		});

		OpenReportsCommand = new RelayCommand(() =>
		{
			var vm = scope.ServiceProvider.GetRequiredService<DailyReportsViewModel>();
			if (CurrentViewModel?.GetType() != typeof(DailyReportsViewModel))
				CurrentViewModel = vm;
		});

		_currentViewModel = scope.ServiceProvider.GetRequiredService<SalesViewModel>();
	}

	public ICommand OpenProductsCommand { get; }
	public ICommand OpenSalesCommand { get; }
	public ICommand OpenInventoryCommand { get; }
	public ICommand OpenReportsCommand { get; }

	public async Task ProcessScan(string code, CancellationToken cancellationToken)
	{
		if (CurrentViewModel is InventoryViewModel inventoryVM)
		{
			await inventoryVM.AddScannedProductToInventory(code, cancellationToken);
		}
		else if(CurrentViewModel is SalesViewModel salesVM)
		{
			await salesVM.ProductScannedAsync(code, cancellationToken);
		}
	}

	partial void OnCurrentViewModelChanged(object value)
	{
		ProductsButtonColor = value is ProductsViewModel ? PrimaryColor : DefaultColor;
		SalesButtonColor = value is SalesViewModel ? PrimaryColor : DefaultColor;
		InventoryButtonColor = value is InventoryViewModel ? PrimaryColor : DefaultColor;
		ReportsButtonColor = value is DailyReportsViewModel ? PrimaryColor : DefaultColor;
	}
}
