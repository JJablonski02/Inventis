using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Inventis.UI.ViewModels;

namespace Inventis.UI.Views;

internal sealed partial class DailyReportsView : UserControl
{
	public DailyReportsView()
	{
		InitializeComponent();
	}

	protected override async void OnLoaded(RoutedEventArgs e)
	{
		if (DataContext is not DailyReportsViewModel dailyReportViewModel)
		{
			return;
		}

		await dailyReportViewModel.OnLoaded();
	}
}
