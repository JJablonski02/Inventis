using System.Linq.Expressions;

namespace Inventis.Domain.Identity.Repositories;

public interface IReadUserRepository
{
	Task<bool> AnyAsync(Expression<Func<User, bool>> expression, CancellationToken cancellationToken);
	Task<User> GetByUsername(string username, CancellationToken cancellationToken);
}
