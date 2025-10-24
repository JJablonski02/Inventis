using System;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.Application;

internal abstract class ScopedServiceBase
{
	private readonly IServiceProvider _serviceProvider;

	protected ScopedServiceBase(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	protected async Task UseScopeAsync(Func<IServiceProvider, Task> action)
	{
		await using var scope = _serviceProvider.CreateAsyncScope();
		await action(scope.ServiceProvider);
	}

	protected async Task<T> UseScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
	{
		await using var scope = _serviceProvider.CreateAsyncScope();
		return await action(scope.ServiceProvider);
	}

	protected void UseScope(Action<IServiceProvider> action)
	{
		using var scope = _serviceProvider.CreateScope();
		action(scope.ServiceProvider);
	}

	protected T UseScope<T>(Func<IServiceProvider, T> func)
	{
		using var scope = _serviceProvider.CreateScope();
		return func(scope.ServiceProvider);
	}
}
