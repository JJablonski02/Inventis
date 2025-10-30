using System.Linq.Expressions;
using Inventis.Application.Exceptions;
using Inventis.Domain.Products;
using Inventis.Domain.Products.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventis.Infrastructure.Products.Repositories;

/// <inheritdoc cref="IReadWriteProductRepository"/>
internal sealed class ReadWriteProductRepository(
	InventisDbContext dbContext) : IReadWriteProductRepository
{
	public async Task AddAndSaveChangesAsync(Product product, CancellationToken cancellationToken)
	{
		await dbContext.Set<Product>().AddAsync(product, cancellationToken);
		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task<IReadOnlyCollection<Product>> WhereAsync(
		Expression<Func<Product, bool>> predicate,
		CancellationToken cancellationToken)
		=> await dbContext.Set<Product>()
		.Where(predicate)
		.ToListAsync(cancellationToken);

	public async Task<IReadOnlyCollection<Product>> WhereWithLogsAsync(
		Expression<Func<Product, bool>> predicate,
		CancellationToken cancellationToken)
		=> await dbContext.Set<Product>()
		.Include(x => x.InventoryMovementLogs)
		.Where(predicate)
		.ToListAsync(cancellationToken);

	public async Task<bool> AnyAsync(
	Expression<Func<Product, bool>> expression,
	CancellationToken cancellationToken)
	=> await dbContext.Set<Product>()
	.AnyAsync(expression, cancellationToken);

	public async Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken)
		=> await dbContext.Set<Product>()
		.ToListAsync(cancellationToken);

	public async Task<Product> GetByIdAsync(Ulid ProductId, CancellationToken cancellationToken)
		=> await dbContext.Set<Product>()
		.Include(x => x.InventoryMovementLogs)
		.SingleOrDefaultAsync(prod => prod.Id == ProductId, cancellationToken)
		?? throw new NotFoundException(nameof(Product));

	public Task SaveChangesAsync(CancellationToken cancellationToken)
		=> dbContext.SaveChangesAsync(cancellationToken);
}
