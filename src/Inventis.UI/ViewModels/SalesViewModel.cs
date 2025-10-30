using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventis.Application.DailyInventoryReports.Dtos;
using Inventis.Application.DailyInventoryReports.Services;
using Inventis.Application.Exceptions;
using Inventis.UI.Handlers.Interfaces;
using Inventis.UI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.UI.ViewModels;

internal sealed partial class SalesViewModel(
	IDailyInventoryReportService dailyInventoryReport,
	IDailyInventoryScanService dailyInventoryScanService,
	IServiceProvider serviceProvider,
	IWindowHandler windowHandler) : ViewModelBase
{
	public ObservableCollection<DailyInventoryScanDto> DailyInventoryScans { get; } = [];

	[ObservableProperty]
	private string? _openedAt;

	[ObservableProperty]
	private bool _isDailyInventoryReportOpen;

	[RelayCommand]
	public async Task OpenDailyInventoryReport()
	{
		try
		{
			await dailyInventoryReport.OpenDailyInventoryReport(CancellationToken.None);

			await RefreshAsync(CancellationToken.None);
		}
		catch (Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>(ex.Message);
		}
	}

	[RelayCommand]
	public async Task CloseDailyInventoryReport()
	{
		try
		{
			var result = await windowHandler.OpenDialogWindow<MainWindow>(
				"Czy na pewno chcesz zamknąć dzienny raport?");

			if (result != Handlers.DialogResult.Yes)
			{
				return;
			}

			await dailyInventoryReport.CloseDailyInventoryReport(CancellationToken.None);

			await RefreshAsync(CancellationToken.None);
		}
		catch (NotFoundException)
		{
			// Nothing to do is required
		}
		catch (Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>(ex.Message);
		}
	}

	public async Task ProductScannedAsync(string eanCode, CancellationToken cancellationToken)
	{
		try
		{
			if (!IsDailyInventoryReportOpen)
			{
				await windowHandler.OpenDialogErrorWindow<MainWindow>("Raport dobowy nie został otwarty!");
				return;
			}

			await dailyInventoryScanService.AddScanAsync(eanCode ,CancellationToken.None);
		}
		catch (NotFoundException)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>(
				"Produkt o podanym kodzie EAN nie istnieje w liście produktów.");
		}
		catch (Exception ex)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>(ex.Message);
		}
		finally
		{
			await RefreshAsync(cancellationToken);
		}
	}

	[RelayCommand]
	public async Task EditRow(object? scanId)
	{
		if (scanId is not Ulid validScanId)
		{
			await windowHandler.OpenDialogErrorWindow<MainWindow>("Niepoprawny identyfikator wybranego skanu.");
			return;
		}

		var scope = serviceProvider.CreateScope();
		var scanRowViewModel = scope.ServiceProvider.GetRequiredService<EditScanRowViewModel>();
		var scan = DailyInventoryScans.Single(scan => scan.Id == validScanId);

		await windowHandler.OpenWindowAsDialog<EditScanRowWindow, MainWindow>(scanRowViewModel, scan);


		await RefreshAsync(CancellationToken.None);
	}

	public Task OnLoaded(CancellationToken cancellationToken)
		=> RefreshAsync(cancellationToken);

	private async Task RefreshAsync(CancellationToken cancellationToken)
	{
		try
		{
			var report = await dailyInventoryReport.GetDailyInventoryReportAsync(cancellationToken);

			DailyInventoryScans.Clear();

			foreach (var scan in report.Scans)
			{
				if (scan.IsDeleted)
				{
					continue;
				}

				DailyInventoryScans.Add(scan);
			}

			OpenedAt = report.CreatedAt.ToString("dd-MMMM-yyyy HH:mm:ss", CultureInfo.CurrentCulture);
			IsDailyInventoryReportOpen = true;
		}
		catch (NotFoundException)
		{
			DailyInventoryScans.Clear();
			OpenedAt = null;
			IsDailyInventoryReportOpen = false;
			// Nothing to do is required
		}
		catch (Exception ex)
		{
			DailyInventoryScans.Clear();
			OpenedAt = null;
			IsDailyInventoryReportOpen = false;
			await windowHandler.OpenDialogErrorWindow<MainWindow>(ex.Message);
		}
	}
}
