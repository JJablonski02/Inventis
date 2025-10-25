using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.UI.ViewModels;

internal sealed partial class HomeViewModel : ViewModelBase
{
	[ObservableProperty]
	private object _currentViewModel;

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

		_currentViewModel = scope.ServiceProvider.GetRequiredService<SalesViewModel>();
	}

	public ICommand OpenProductsCommand { get; }
	public ICommand OpenSalesCommand { get; }
	public ICommand OpenInventoryCommand { get; }

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
}
