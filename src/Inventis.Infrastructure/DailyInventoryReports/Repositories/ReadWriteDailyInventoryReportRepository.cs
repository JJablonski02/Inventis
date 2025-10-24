using System.Linq.Expressions;
using Inventis.Application.Exceptions;
using Inventis.Domain.DailyInventoryReports;
using Inventis.Domain.DailyInventoryReports.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventis.Infrastructure.DailyInventoryReports.Repositories;

internal sealed class ReadWriteDailyInventoryReportRepository(
	InventisDbContext dbContext) : IReadWriteDailyInventoryReportRepository
{
	public async Task<DailyInventoryReport> SingleOrDefaultAsync(
		Expression<Func<DailyInventoryReport, bool>> expression,
		CancellationToken cancellationToken)
		=> await dbContext.Set<DailyInventoryReport>().SingleOrDefaultAsync(expression, cancellationToken)
			?? throw new NotFoundException(nameof(DailyInventoryReport));

	public async Task<bool> AnyAsync(
		Expression<Func<DailyInventoryReport, bool>> expression,
		CancellationToken cancellationToken)
		=> await dbContext.Set<DailyInventoryReport>().AnyAsync(expression, cancellationToken);
}
