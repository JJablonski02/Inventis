namespace Inventis.Domain.DailyInventoryReports.Repositories;

public interface IReadWriteDailyInventoryReportRepository : IReadDailyInventoryReportRepository
{
	Task AddAndSaveChangesAsync(DailyInventoryReport report, CancellationToken cancellationToken);

	Task SaveChangesAsync(CancellationToken cancellationToken);
}
