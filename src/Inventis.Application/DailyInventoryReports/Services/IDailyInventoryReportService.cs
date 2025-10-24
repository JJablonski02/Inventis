using Inventis.Application.DailyInventoryReports.Dtos;
using Inventis.Domain.DailyInventoryReports.Repositories;
using Inventis.Domain.Products.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.Application.DailyInventoryReports.Services;

public interface IDailyInventoryReportService
{
	Task<DailyInventoryReportDto> GetDailyInventoryReportAsync(CancellationToken cancellationToken);
	Task OpenDailyInventoryReport(CancellationToken cancellationToken);
	Task CloseDailyInventoryReport(CancellationToken cancellationToken);
}

internal sealed class DailyInventoryReportService(IServiceProvider serviceProvider)
	: ScopedServiceBase(serviceProvider), IDailyInventoryReportService
{
	public async Task OpenDailyInventoryReport(CancellationToken cancellationToken)
		=> await UseScopeAsync(async sp =>
		{
			var repo = sp.GetRequiredService<IReadWriteDailyInventoryReportRepository>();

			if (await repo.AnyAsync(r => !r.IsClosed, cancellationToken))
			{
				throw new InvalidOperationException("Daily inventory report is already open.");
			}
		});

	public async Task CloseDailyInventoryReport(CancellationToken cancellationToken)
		=> await UseScopeAsync(async sp =>
		{
			var dailyInventoryReportRepository = sp.GetRequiredService<IReadWriteDailyInventoryReportRepository>();

			var dailyInventoryReport = await dailyInventoryReportRepository.SingleOrDefaultAsync(
			y => !y.IsClosed,
			cancellationToken);

			dailyInventoryReport.CloseReport();
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
				[.. dailyInventoryReport.DailyScans.Select(scan =>
			{
				var product = products.Single(prod => prod.Id == scan.ProductId);

				return new DailyInventoryScanDto(
					scan.Id,
					product.Name,
					product.GrossSalePrice,
					scan.ScanTime);
			})]);
		});
}
