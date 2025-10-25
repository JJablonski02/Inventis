using Inventis.Windows.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class WindowsConfiguration
{
	public static IServiceCollection RegisterWindowsConfiguration(this IServiceCollection services)
		=> services
		.AddTransient<IPrintingService, PrintingService>();
}
