using Inventis.Domain.Inventories.Constants;
using Inventis.Domain.Products.Constants;
using Inventis.Infrastructure.DailyInventoryReports.EntityConfigurations;
using Inventis.Infrastructure.Identity.EntityConfiguration;
using Inventis.Infrastructure.Inventories;
using Inventis.Infrastructure.Inventories.EntityConfiguration;
using Inventis.Infrastructure.Postgres;
using Inventis.Infrastructure.Products;
using Inventis.Infrastructure.Products.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Inventis.Infrastructure;

/// <summary>
/// Inventis db context
/// </summary>
/// <param name="dbContextOptions">Db context options</param>
internal sealed class InventisDbContext(
	DbContextOptions<InventisDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("inventis");

		modelBuilder.ApplyConfiguration(new UserConfiguration());
		modelBuilder.ApplyConfiguration(new InventoryConfiguration());
		modelBuilder.ApplyConfiguration(new DailyInventoryReportsConfiguration());
		modelBuilder.ApplyConfiguration(new ProductConfiguration());
		modelBuilder.ApplyConfiguration(new ProductInventoryMovementLogConfiguration());
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		// Ulid conversion
		configurationBuilder
			.Properties<Ulid>()
			.HaveConversion<UlidToStringConverter>();

		configurationBuilder
			.Properties<InventoryType>()
			.HaveConversion<InventoryTypeValueConverter>();
		
		configurationBuilder
			.Properties<InventoryMovementLogDirection>()
			.HaveConversion<InventoryMovementLogDirectionValueConverter>();

		configurationBuilder
			.Properties<InventoryMovementLogAction>()
			.HaveConversion<InventoryMovementLogActionValueConverter>();

		configurationBuilder
			.Properties<QuantityType>()
			.HaveConversion<QuantityTypeValueConverter>();
	}
}
