using Avalonia.Controls;
using Avalonia.Interactivity;
using Inventis.UI.Handlers;

namespace Inventis.UI.Views;

internal sealed partial class DialogWindow : Window
{
    public DialogWindow()
    {
        InitializeComponent();
    }

    private const int WindowHeight = 240;

    public DialogWindow(string message)
    {
        InitializeComponent();
        DialogText.Text = message;
        Height = WindowHeight;
    }

    private void No_OnClick(object? sender, RoutedEventArgs e)
    {
        Close(DialogResult.No);
    }

    private void Yes_OnClick(object? sender, RoutedEventArgs e)
    {
        Close(DialogResult.Yes);
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        Close(DialogResult.Cancel);
    }

	/// <summary>
	/// Set custom DialogWindow height
	/// </summary>
	/// <param name="window">DialogWindow</param>
	/// <param name="height">Height</param>
	public void SetHeight(int? height)
	{
		if (height is null)
        {
            return;
        }

		Height = height.Value;
	}

	/// <summary>
	/// Reset DialogWindow height to default value
	/// </summary>
	/// <param name="window">DialogWindow</param>
	public void ResetHeight()
    {
        Height = WindowHeight;
    }
}
