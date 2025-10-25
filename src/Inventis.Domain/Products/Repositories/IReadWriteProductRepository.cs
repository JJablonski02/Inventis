namespace Inventis.Domain.Products.Repositories;

public interface IReadWriteProductRepository : IReadProductRepository
{
	Task SaveChangesAsync(CancellationToken cancellationToken);
	Task AddAndSaveChangesAsync(Product product, CancellationToken cancellationToken);
}
