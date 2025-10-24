using System.Linq.Expressions;

namespace Inventis.Domain.Products.Repositories;

/// <summary>
/// Read product repository
/// </summary>
public interface IReadProductRepository
{
	Task<IReadOnlyCollection<Product>> WhereAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken);
	Task<bool> AnyAsync(Expression<Func<Product, bool>> expression, CancellationToken cancellationToken);
}
