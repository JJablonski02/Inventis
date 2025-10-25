using Inventis.Domain.DailyInventoryReports.Repositories;
using Inventis.Domain.Identity.Repositories;
using Inventis.Domain.Inventories.Repositories;
using Inventis.Domain.Products.Repositories;
using Inventis.Infrastructure;
using Inventis.Infrastructure.Configuration;
using Inventis.Infrastructure.DailyInventoryReports.Repositories;
using Inventis.Infrastructure.Identity.Repositories;
using Inventis.Infrastructure.Inventories.Repositories;
using Inventis.Infrastructure.Products.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureConfiguration
{
	public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
		=> services.RegisterPostgresServer()
		.RegisterRepositories();

	private static IServiceCollection RegisterPostgresServer(this IServiceCollection services)
		=> services.AddPostgresServer<InventisDbContext>("__InventisMigrationsHistory", "inventis");

	private static IServiceCollection RegisterRepositories(this IServiceCollection services)
		=> services
		.AddTransient<IReadUserRepository, ReadWriteUserRepository>()
		.AddTransient<IReadWriteUserRepository, ReadWriteUserRepository>()
		.AddTransient<IReadInventoriesRepository, ReadWriteInventoriesRepository>()
		.AddTransient<IReadWriteInventoriesRepository, ReadWriteInventoriesRepository>()
		.AddTransient<IReadProductRepository, ReadWriteProductRepository>()
		.AddTransient<IReadWriteProductRepository, ReadWriteProductRepository>()
		.AddTransient<IReadWriteDailyInventoryReportRepository, ReadWriteDailyInventoryReportRepository>()
		.AddTransient<IReadDailyInventoryReportRepository, ReadWriteDailyInventoryReportRepository>();

	private static IServiceCollection AddPostgresServer<TContext>(
		this IServiceCollection serviceCollection,
		string migrationsHistoryTableName,
		string? schema = null) where TContext : DbContext
	{
		serviceCollection.AddDbContext<TContext>((serviceProvider, options) =>
		{
			var inventisOptions = serviceProvider.GetRequiredService<IOptions<InventisOptions>>().Value;
			options.UseNpgsql(inventisOptions.ConnectionString,
				x => x.MigrationsHistoryTable(migrationsHistoryTableName, schema));
		});

		return serviceCollection;
	}
}
