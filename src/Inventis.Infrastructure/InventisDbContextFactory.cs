using Inventis.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Inventis.Infrastructure;

internal sealed class InventisDbContextFactory : IDesignTimeDbContextFactory<InventisDbContext>
{
    public InventisDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<InventisDbContext>();
        var configuration = config.GetSection(nameof(InventisOptions));

		var connectionString = configuration.GetConnectionString("ConnectionString");

        optionsBuilder.UseNpgsql(connectionString,
            x => x.MigrationsHistoryTable("__InventisMigrationsHistory", "inventis"));

        return new InventisDbContext(optionsBuilder.Options);
    }
}
