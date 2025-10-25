using Inventis.Application.DailyInventoryReports.Services;
using Inventis.Application.Identity.Services;
using Inventis.Application.Inventories.Services;
using Inventis.Application.Products.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationServicesConfiguration
{
	public static IServiceCollection ConfigureApplicationServices(
		this IServiceCollection services)
		=> services.AddTransient<IIdentityService, IdentityService>()
			.AddTransient<IInventoryService, InventoryService>()
			.AddTransient<IProductService, ProductService>()
			.AddTransient<IDailyInventoryReportService, DailyInventoryReportService>()
			.AddTransient<IDailyInventoryScanService, DailyInvnentoryScanService>();
}
