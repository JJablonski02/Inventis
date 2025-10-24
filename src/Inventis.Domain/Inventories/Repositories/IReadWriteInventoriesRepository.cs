namespace Inventis.Domain.Inventories.Repositories;

public interface IReadWriteInventoriesRepository : IReadInventoriesRepository
{
	Task AddAndSaveChangesAsync(Inventory inventory, CancellationToken cancellationToken);
}
