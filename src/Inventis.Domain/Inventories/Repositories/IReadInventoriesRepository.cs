using System.Linq.Expressions;

namespace Inventis.Domain.Inventories.Repositories;

public interface IReadInventoriesRepository
{
	Task<Inventory> SingleOrDefaultAsync(Expression<Func<Inventory, bool>> expression, CancellationToken cancellationToken);
	Task<bool> AnyAsync(Expression<Func<Inventory, bool>> expression, CancellationToken cancellationToken);
}
