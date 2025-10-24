using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Inventis.UI.ViewModels;
using Inventis.UI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.UI;

public partial class App : Avalonia.Application
{
	/// <summary>
	/// Service provider
	/// </summary>
	private readonly IServiceProvider _serviceProvider;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="serviceProvider">Service provider</param>
	public App(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	public App() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
		var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
