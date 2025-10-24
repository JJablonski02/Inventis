using System.Linq.Expressions;
using Inventis.Application.Exceptions;
using Inventis.Domain.Identity;
using Inventis.Domain.Identity.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventis.Infrastructure.Identity.Repositories;

internal sealed class ReadWriteUserRepository(
	InventisDbContext dbContext) : IReadWriteUserRepository
{
	public void Add(User user)
		=> dbContext.Set<User>().Add(user);

	public Task<bool> AnyAsync(
		Expression<Func<User, bool>> expression,
		CancellationToken cancellationToken)
		=> dbContext.Set<User>().AnyAsync(expression, cancellationToken);

	public async Task<User> GetByUsername(string username, CancellationToken cancellationToken)
		=> await dbContext.Set<User>().SingleOrDefaultAsync(user => user.Username == username)
			?? throw new NotFoundException(nameof(User));
}
