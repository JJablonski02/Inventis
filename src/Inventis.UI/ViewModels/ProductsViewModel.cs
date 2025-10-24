using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Inventis.Application.Products.Dtos;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.UI.ViewModels;

internal sealed partial class ProductsViewModel(
	IServiceProvider serviceProvider,
	IWindowHandler windowHandler) : ViewModelBase
{
	public ObservableCollection<ProductDto> Products { get; } = new()
	{
		new ProductDto(Ulid.NewUlid(), "Coca-Cola", "Napój gazowany", "123456", 2.5m, 3.0m, 3.8m, 4.5m, 300, 450, 100, 23, "Coca-Cola HBC", "kontakt@cocacola.pl")
	};

	[RelayCommand]
	public async Task AddProduct()
	{
		var service = serviceProvider.CreateScope();

		var viewModel = service.ServiceProvider.GetRequiredService<ProductViewModel>();

		await windowHandler.OpenWindowAsDialog<ProductWindow, MainWindow>(viewModel);
	}
}
