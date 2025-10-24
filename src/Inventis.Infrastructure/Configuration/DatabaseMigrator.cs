using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.Infrastructure.Configuration;

[SuppressMessage(
	"Globalization",
	"CA1303:Do not pass literals as localized parameters",
	Justification = "This class is not going to be localized")]
public static class DatabaseMigrator
{
	public static async Task MigrateAsync(IServiceScopeFactory serviceScopeFactory, CancellationToken cancellationToken = default)
	{
		using var scope = serviceScopeFactory.CreateScope();
		var databaseContext = scope.ServiceProvider.GetRequiredService<InventisDbContext>();

		try
		{
			var pendingMigrations = await databaseContext.Database.GetPendingMigrationsAsync(cancellationToken);

			if (pendingMigrations.Any())
			{
				Console.WriteLine("Pending migrations found: " + string.Join(", ", pendingMigrations));
				await databaseContext.Database.MigrateAsync(cancellationToken);
				Console.WriteLine("Database migrations applied successfully.");
			}
			else
			{
				Console.WriteLine("No pending migrations.");
			}
		}
		catch (Exception e)
		{
			Console.WriteLine("ERROR applying database migrations: " + e);
			throw;
		}
	}
}
