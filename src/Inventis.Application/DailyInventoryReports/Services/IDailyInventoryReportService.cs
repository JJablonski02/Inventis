using System.Linq;
using Inventis.Application.DailyInventoryReports.Dtos;
using Inventis.Application.Exceptions;
using Inventis.Domain.DailyInventoryReports;
using Inventis.Domain.DailyInventoryReports.Repositories;
using Inventis.Domain.Products.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.Application.DailyInventoryReports.Services;

public interface IDailyInventoryReportService
{
	Task<DailyInventoryReportDto> GetDailyInventoryReportAsync(CancellationToken cancellationToken);

	Task<IReadOnlyCollection<DailyInventoryReportDto>> GetAllAsync(CancellationToken cancellationToken);
	Task OpenDailyInventoryReport(CancellationToken cancellationToken);
	Task CloseDailyInventoryReport(CancellationToken cancellationToken);
}

internal sealed class DailyInventoryReportService(IServiceProvider serviceProvider)
	: ScopedServiceBase(serviceProvider), IDailyInventoryReportService
{
	public async Task OpenDailyInventoryReport(CancellationToken cancellationToken)
		=> await UseScopeAsync(async sp =>
		{
			var dailyInventoryReportRepository = sp.GetRequiredService<IReadWriteDailyInventoryReportRepository>();

			if (await dailyInventoryReportRepository.AnyAsync(r => !r.IsClosed, cancellationToken))
			{
				throw new InvalidOperationException("Dzienny raport jest już otwarty.");
			}

			var dailyInventoryReport = DailyInventoryReport.Create();

			await dailyInventoryReportRepository.AddAndSaveChangesAsync(dailyInventoryReport, cancellationToken);
		});

	public async Task CloseDailyInventoryReport(CancellationToken cancellationToken)
		=> await UseScopeAsync(async sp =>
		{
			var dailyInventoryReportRepository = sp.GetRequiredService<IReadWriteDailyInventoryReportRepository>();
			var productsRepository = sp.GetRequiredService<IReadProductRepository>();

			var dailyInventoryReport = await dailyInventoryReportRepository.SingleOrDefaultAsync(
			y => !y.IsClosed,
			cancellationToken);

			var productIds = dailyInventoryReport.DailyScans
				.Select(scan => scan.ProductId)
				.Distinct()
				.ToArray();

			var products = await productsRepository.WhereWithLogsAsync(product => productIds.Contains(product.Id), cancellationToken);

			foreach (var item in products)
			{
				item.ReconcileInventory(dailyInventoryReport.CreatedAt);
			}

			dailyInventoryReport.CloseReport();

			await dailyInventoryReportRepository.SaveChangesAsync(cancellationToken);
		});

	public async Task<DailyInventoryReportDto> GetDailyInventoryReportAsync(
		CancellationToken cancellationToken)
		=> await UseScopeAsync<DailyInventoryReportDto>(async sp =>
		{
			var dailyInventoryReportRepository = sp.GetRequiredService<IReadWriteDailyInventoryReportRepository>();
			var readProductRepository = sp.GetRequiredService<IReadProductRepository>();

			var dailyInventoryReport = await dailyInventoryReportRepository.SingleOrDefaultAsync(
				y => !y.IsClosed,
				cancellationToken);

			var productIds = dailyInventoryReport.DailyScans.Select(y => y.ProductId).ToList();

			var products = await readProductRepository.WhereAsync(
				product => productIds.Contains(product.Id),
				cancellationToken);

			return new(
				dailyInventoryReport.Id,
				dailyInventoryReport.CreatedAt,
				dailyInventoryReport.ClosedAt,
				[.. dailyInventoryReport.DailyScans
				.OrderByDescending(scan => scan.ScanTime)
				.Select(scan =>
			{
				var product = products.Single(prod => prod.Id == scan.ProductId);

				return new DailyInventoryScanDto(
					scan.Id,
					product.Id,
					product.Name,
					product.GrossSalePrice,
					scan.IsDeleted,
					scan.ScanTime);
			})]);
		});

	public async Task<IReadOnlyCollection<DailyInventoryReportDto>> GetAllAsync(CancellationToken cancellationToken)
		=> await UseScopeAsync(async (sp) =>
		{
			var dailyInventoryReportRepository = sp.GetRequiredService<IReadDailyInventoryReportRepository>();

			var reports = await dailyInventoryReportRepository.GetAllAsync(cancellationToken);

			return reports.Select(report => new DailyInventoryReportDto(
				report.Id,
				report.CreatedAt,
				report.ClosedAt,
				[]))
			.OrderByDescending(y => y.CreatedAt)
			.ToList();
		});
}
