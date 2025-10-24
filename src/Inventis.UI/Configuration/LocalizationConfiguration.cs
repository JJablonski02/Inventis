using System.Globalization;

namespace Inventis.UI.Configuration;

/// <summary>
/// Class that provides localization configuration for entire application
/// </summary>
public static class LocalizationConfiguration
{
	/// <summary>
	/// Culture configuration for entire app
	/// </summary>
	public static void ConfigureCulture()
	{
		var cultureInfo = new CultureInfo("pl-PL");

		CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
		CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
	}
}
