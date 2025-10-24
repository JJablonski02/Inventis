using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.Templates;

namespace Inventis.UI.Converters;

/// <summary>
/// A value converter that selects a top bar template based on a boolean value representing the user's authentication status.
/// </summary>
internal sealed class TopBarTemplateConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets the DataTemplate for the top bar when the user is authenticated (logged in).
    /// </summary>
    public DataTemplate? TopBarTemplateLoggedIn { get; set; }

    /// <summary>
    /// Gets or sets the DataTemplate for the top bar when the user is not authenticated (logged out).
    /// </summary>
    public DataTemplate? TopBarTemplateLoggedOut { get; set; }

    /// <summary>
    /// Converts the provided value to the corresponding DataTemplate based on the user's authentication status. <br/>
    /// If the user is authenticated (logged in), it returns the <see cref="TopBarTemplateLoggedIn"/> which represents the top bar <br/>
    /// template for logged-in users. If the user is not authenticated (logged out), it returns the <see cref="TopBarTemplateLoggedOut"/>
    /// </summary>
    /// <param name="value">The value to convert (expected to be a boolean).</param>
    /// <param name="targetType">The type to convert to (not used in this implementation).</param>
    /// <param name="parameter">An optional parameter (not used in this implementation).</param>
    /// <param name="culture">The culture information (not used in this implementation).</param>
    /// <returns>
    /// The selected DataTemplate based on the boolean value.
    /// Returns null if the value is not a boolean.
    /// </returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Check if the value is a boolean
        if (value is bool booleanValue)
        {
            // Returns top bar template for logged in user if true, otherwise top bar template for logged out user.
            return booleanValue ? TopBarTemplateLoggedIn : TopBarTemplateLoggedOut;
        }

        // Return null if the value is not a boolean
        return null;
    }

    /// <summary>
    /// Converts back is not implemented and will throw a NotImplementedException if called.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The target type to convert to (not used).</param>
    /// <param name="parameter">An optional parameter (not used).</param>
    /// <param name="culture">The culture information (not used).</param>
    /// <returns>Throws NotImplementedException.</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
