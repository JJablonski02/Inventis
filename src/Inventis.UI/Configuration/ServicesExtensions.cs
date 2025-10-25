using Inventis.Infrastructure.Configuration;
using Inventis.UI.Handlers;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.Models;
using Inventis.UI.Router;
using Inventis.UI.Services;
using Inventis.UI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace Inventis.UI.Configuration;

internal static class ServicesExtensions
{
	internal static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		=> services
		.AddSingleton<IHistoryRouter<ViewModelBase>>(s => new HistoryRouter<ViewModelBase>(t => (ViewModelBase)s.GetRequiredService(t)))
		.AddSingleton<ISessionService, SessionService>()
		.AddSingleton<MainViewModel>()
		.AddSingleton<UiState>()
		.ConfigureViewModels()
		.AddHandlersConfiguration()
		.ConfigureApplicationServices()
		.ConfigureInfrastructure(configuration);

	/// <summary>
	/// Configures view models
	/// </summary>
	/// <param name="services">Services collection</param>
	/// <returns>Services collection</returns>
	private static IServiceCollection ConfigureViewModels(this IServiceCollection services)
		=> services
		.AddTransient<LoginViewModel>()
		.AddTransient<HomeViewModel>()
		.AddTransient<ProductsViewModel>()
		.AddTransient<ProductViewModel>()
		.AddTransient<SalesViewModel>()
		.AddTransient<InventoryViewModel>()
		.AddTransient<PrintEanViewModel>()
		.AddTransient<EditScanRowViewModel>();

	private static IServiceCollection AddHandlersConfiguration(this IServiceCollection services)
		=> services.AddTransient<IWindowHandler, WindowHandler>()
		.AddTransient<IPrinterService, PrinterService>();

	private static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		QuestPDF.Settings.License = LicenseType.Community;

		services.AddSingleton(configuration);
		services.Configure<InventisOptions>(configuration.BindFromSection);
		services.RegisterInfrastructure();

		return services;
	}
}
