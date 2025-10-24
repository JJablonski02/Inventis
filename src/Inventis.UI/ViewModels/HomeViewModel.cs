using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventis.Application.DailyInventoryReports.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.UI.ViewModels;

internal sealed partial class HomeViewModel : ViewModelBase
{
	[ObservableProperty]
	private object _currentViewModel;

	public HomeViewModel(IServiceProvider serviceProvider)
	{
		var scope = serviceProvider.CreateScope();

		OpenProductsCommand = new RelayCommand(() => CurrentViewModel = scope.ServiceProvider.GetRequiredService<ProductsViewModel>());
		OpenSalesCommand = new RelayCommand(() => CurrentViewModel = scope.ServiceProvider.GetRequiredService<SalesViewModel>());
		OpenInventoryCommand = new RelayCommand(() => CurrentViewModel = scope.ServiceProvider.GetRequiredService<InventoryViewModel>());

		_currentViewModel = scope.ServiceProvider.GetRequiredService<ProductsViewModel>();
	}

	public ObservableCollection<DailyInventoryScanDto> DailyInventoryScans { get; } = new()
	{
		new DailyInventoryScanDto(Ulid.NewUlid(), "Coca-Cola 0.5L", 4.50m, DateTime.Now.AddMinutes(-15)),
		new DailyInventoryScanDto(Ulid.NewUlid(), "Pepsi 1L", 5.20m, DateTime.Now.AddMinutes(-30)),
		new DailyInventoryScanDto(Ulid.NewUlid(), "Sprite 0.5L", 4.30m, DateTime.Now.AddMinutes(-60))
	};

	public ICommand OpenProductsCommand { get; }
	public ICommand OpenSalesCommand { get; }
	public ICommand OpenInventoryCommand { get; }
}
