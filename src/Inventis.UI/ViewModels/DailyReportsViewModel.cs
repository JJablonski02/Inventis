using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Inventis.Application.DailyInventoryReports.Dtos;
using Inventis.Application.DailyInventoryReports.Services;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.Views;

namespace Inventis.UI.ViewModels;

internal sealed partial class DailyReportsViewModel(
	IDailyInventoryReportService dailyInventoryReportService,
	IWindowHandler windowHandler) : ViewModelBase
{
	public ObservableCollection<DailyInventoryReportDto> Reports { get; } = [];

	[ObservableProperty]
	private string? _productName;

	public async Task OnLoaded()
	{
		try
		{
			var reports = await dailyInventoryReportService.GetAllAsync(CancellationToken.None);

			Reports.Clear();

			foreach (var report in reports)
			{
				Reports.Add(report);
			}
		}
		catch (Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>(ex.Message);
		}
	}
}
