using System.Linq.Expressions;
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

	public async Task<bool> AnyAsync(
	Expression<Func<Product, bool>> expression,
	CancellationToken cancellationToken)
	=> await dbContext.Set<Product>()
	.AnyAsync(expression, cancellationToken);
}
