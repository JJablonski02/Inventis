using Inventis.Application.Identity.Dtos;
using Inventis.Domain.Identity.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.Application.Identity.Services;

/// <summary>
/// Identity service
/// </summary>
public interface IIdentityService
{
	Task<CurrentUserDto> HandleLoginAsync(string username, string password, CancellationToken cancellationToken);
}

internal sealed class IdentityService(
	IServiceProvider serviceProvider) : ScopedServiceBase(serviceProvider), IIdentityService
{
	public Task<CurrentUserDto> HandleLoginAsync(string username, string password, CancellationToken cancellationToken)
		=> UseScopeAsync<CurrentUserDto>(async(sc) =>
		{
			var userRepository = sc.GetRequiredService<IReadUserRepository>();

			var user = await userRepository.GetByUsername(username, cancellationToken);

			if (user.Password != password)
			{
				throw new InvalidOperationException("Invalid password.");
			}

			return CurrentUserDto.Create(
				user.Id,
				user.Username,
				user.FirstName,
				user.LastName);
		});
}
