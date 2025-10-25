using System.Linq.Expressions;
using Inventis.Application.Exceptions;
using Inventis.Domain.Inventories;
using Inventis.Domain.Inventories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventis.Infrastructure.Inventories.Repositories;

internal sealed class ReadWriteInventoriesRepository(
	InventisDbContext dbContext) : IReadWriteInventoriesRepository
{
	public async Task AddAndSaveChangesAsync(Inventory inventory, CancellationToken cancellationToken)
	{
		await dbContext.Set<Inventory>().AddAsync(inventory, cancellationToken);
		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task<Inventory> SingleOrDefaultAsync(
		Expression<Func<Inventory, bool>> expression,
		CancellationToken cancellationToken)
		=> await dbContext.Set<Inventory>()
		.SingleOrDefaultAsync(expression, cancellationToken)
		?? throw new NotFoundException(nameof(Inventory));

	public async Task<bool> AnyAsync(
		Expression<Func<Inventory, bool>> expression,
		CancellationToken cancellationToken)
		=> await dbContext.Set<Inventory>()
		.AnyAsync(expression, cancellationToken);

	public async Task SaveChangesAsync(CancellationToken cancellationToken)
		=> await dbContext.SaveChangesAsync(cancellationToken);
}
