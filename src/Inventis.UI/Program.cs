using Avalonia;
using Avalonia.Controls;
using Inventis.Infrastructure.Configuration;
using Inventis.UI.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.UI;

internal sealed class Program
{
    private Program()
    {
    }

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
		//Culture configuration
		LocalizationConfiguration.ConfigureCulture();

		 BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
	}

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
		string appSettingsPath = Directory.GetCurrentDirectory();

		if (Design.IsDesignMode)
		{
			appSettingsPath += "\\src\\Inventis.UI";
		}

		var configuration = new ConfigurationBuilder()
			.SetBasePath(appSettingsPath)
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

		var services = new ServiceCollection();

		services.ConfigureServices(configuration);

		var servicesBuilder = services.BuildServiceProvider();

		// Database migrator
		var scopeFactory = servicesBuilder.GetRequiredService<IServiceScopeFactory>();
		DatabaseMigrator.MigrateAsync(scopeFactory).GetAwaiter().GetResult();

		return AppBuilder.Configure(() => new App(servicesBuilder))
			.UsePlatformDetect()
			.WithInterFont()
			.LogToTrace();
	}
}
