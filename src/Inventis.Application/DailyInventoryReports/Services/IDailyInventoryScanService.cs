using Inventis.Application.Exceptions;
using Inventis.Domain.DailyInventoryReports;
using Inventis.Domain.DailyInventoryReports.Repositories;
using Inventis.Domain.Products.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.Application.DailyInventoryReports.Services;

/// <summary>
/// Daily inventory scan service
/// </summary>
public interface IDailyInventoryScanService
{
	Task AddScanAsync(string eanCode, CancellationToken cancellationToken);

	Task DeleteScan(Ulid scanId, CancellationToken cancellationToken);

	Task DeleteSoftScan(Ulid scanId, CancellationToken cancellationToken);
}

/// <inheritdoc cref="IDailyInventoryScanService"/>
internal sealed class DailyInvnentoryScanService(IServiceProvider serviceProvider)
	: ScopedServiceBase(serviceProvider), IDailyInventoryScanService
{
	public Task AddScanAsync(string eanCode, CancellationToken cancellationToken)
		=> UseScopeAsync(async (scope) =>
		{
			var dailyInventoryReportRepository =
				scope.GetRequiredService<IReadWriteDailyInventoryReportRepository>();

			var dailyInventoryReport = await dailyInventoryReportRepository
					.SingleOrDefaultAsync(report => !report.IsClosed, cancellationToken)
					?? throw new NotFoundException(nameof(DailyInventoryReport));

			var productsRepository =
				scope.GetRequiredService<IReadProductRepository>();

			var product = (await productsRepository.WhereAsync(
				y => y.EanCode == eanCode,
				cancellationToken))
				.SingleOrDefault()
				?? throw new NotFoundException("Product");

			dailyInventoryReport.AddScan(product.Id);

			await dailyInventoryReportRepository.SaveChangesAsync(cancellationToken);
		});

	public Task DeleteScan(Ulid scanId, CancellationToken cancellationToken)
		=> UseScopeAsync(async (scope) =>
		{
			var dailyInventoryReportRepository =
				scope.GetRequiredService<IReadWriteDailyInventoryReportRepository>();

			var dailyInventoryReport = await dailyInventoryReportRepository
					.SingleOrDefaultAsync(report => !report.IsClosed, cancellationToken)
					?? throw new NotFoundException(nameof(DailyInventoryReport));

			dailyInventoryReport.DeleteScan(scanId);

			await dailyInventoryReportRepository.SaveChangesAsync(cancellationToken);
		});

	public Task DeleteSoftScan(Ulid scanId, CancellationToken cancellationToken)
	=> UseScopeAsync(async (scope) =>
	{
		var dailyInventoryReportRepository =
			scope.GetRequiredService<IReadWriteDailyInventoryReportRepository>();

		var dailyInventoryReport = await dailyInventoryReportRepository
				.SingleOrDefaultAsync(report => !report.IsClosed, cancellationToken)
				?? throw new NotFoundException(nameof(DailyInventoryReport));

		dailyInventoryReport.DeleteSoftScan(scanId);

		await dailyInventoryReportRepository.SaveChangesAsync(cancellationToken);
	});
}
