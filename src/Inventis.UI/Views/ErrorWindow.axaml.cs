using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Inventis.UI.Views;

/// <summary>
/// Error preview window
/// </summary>
public partial class ErrorWindow : Window
{
    public ErrorWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public ErrorWindow(string message)
    {
        InitializeComponent();
        ErrorText.Text = message;
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
        => Close();
}
