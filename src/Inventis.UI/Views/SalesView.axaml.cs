using Avalonia.Controls;
using Avalonia.Interactivity;
using Inventis.UI.ViewModels;

namespace Inventis.UI.Views;

internal sealed partial class SalesView : UserControl
{
    public SalesView()
    {
        InitializeComponent();
    }

	protected override void OnLoaded(RoutedEventArgs e)
	{
		if (DataContext is SalesViewModel vm)
		{
			vm.OnLoaded(CancellationToken.None).ConfigureAwait(false);
		}
	}
}
