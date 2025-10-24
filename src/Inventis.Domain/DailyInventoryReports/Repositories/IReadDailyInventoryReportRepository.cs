using System.Linq.Expressions;

namespace Inventis.Domain.DailyInventoryReports.Repositories;

/// <summary>
/// Read daily inventory report repository
/// </summary>
public interface IReadDailyInventoryReportRepository
{
	Task<DailyInventoryReport> SingleOrDefaultAsync(Expression<Func<DailyInventoryReport, bool>> expression, CancellationToken cancellationToken);
	Task<bool> AnyAsync(Expression<Func<DailyInventoryReport, bool>> expression, CancellationToken cancellationToken);
}
