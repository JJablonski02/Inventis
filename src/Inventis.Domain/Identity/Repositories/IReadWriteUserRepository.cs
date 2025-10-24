namespace Inventis.Domain.Identity.Repositories;

public interface IReadWriteUserRepository : IReadUserRepository
{
	void Add(User user);
}
