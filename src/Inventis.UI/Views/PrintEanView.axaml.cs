using Avalonia.Controls;
using Avalonia.Interactivity;
using Inventis.UI.ViewModels;

namespace Inventis.UI.Views;

internal sealed partial class PrintEanView : UserControl
{
	public PrintEanView()
	{
		InitializeComponent();
	}

	protected override void OnLoaded(RoutedEventArgs e)
	{
		if (DataContext is not PrintEanViewModel vm)
		{
			return;
		}

		vm.OnLoaded();
	}
}
