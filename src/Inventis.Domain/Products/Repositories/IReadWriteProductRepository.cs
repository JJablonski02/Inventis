namespace Inventis.Domain.Products.Repositories;

public interface IReadWriteProductRepository : IReadProductRepository
{
	Task AddAndSaveChangesAsync(Product product, CancellationToken cancellationToken);
}
