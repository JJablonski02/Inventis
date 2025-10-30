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

			var product = (await productsRepository.WhereWithLogsAsync(
				y => y.EanCode == eanCode,
				cancellationToken))
				.SingleOrDefault()
				?? throw new NotFoundException("Product");

			var scanId = dailyInventoryReport.AddScan(product.Id);

			product.DecreaseSingleQuantityAutomatically(scanId);

			await dailyInventoryReportRepository.SaveChangesAsync(cancellationToken);
		});

	public Task DeleteScan(Ulid scanId, CancellationToken cancellationToken)
		=> UseScopeAsync(async (scope) =>
		{
			var dailyInventoryReportRepository =
				scope.GetRequiredService<IReadWriteDailyInventoryReportRepository>();

			var productRepository =
				scope.GetRequiredService<IReadWriteProductRepository>();

			var dailyInventoryReport = await dailyInventoryReportRepository
					.SingleOrDefaultAsync(report => !report.IsClosed, cancellationToken)
					?? throw new NotFoundException(nameof(DailyInventoryReport));

			var product = (await productRepository.WhereWithLogsAsync(product => product.InventoryMovementLogs.Any(log => log.ScanId == scanId), cancellationToken))
				.SingleOrDefault()
				?? throw new InvalidOperationException("Nie znaleziono produktu z powiązanym wpisem");

			product.IncreaseSingleQuantity(scanId);

			dailyInventoryReport.DeleteScan(scanId);

			await productRepository.SaveChangesAsync(cancellationToken);
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
