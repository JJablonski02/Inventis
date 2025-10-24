namespace Inventis.Application.DailyInventoryReports.Services;

/// <summary>
/// Daily inventory scan service
/// </summary>
public interface IDailyInventoryScanService
{
	Task AddScan(string eanCode, CancellationToken cancellationToken);

	Task DeleteScan(Ulid scanId, CancellationToken cancellationToken);
}

/// <inheritdoc cref="IDailyInventoryScanService"/>
internal sealed class DailyInvnentoryScanService() : IDailyInventoryScanService
{
	public Task AddScan(string eanCode, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public Task DeleteScan(Ulid scanId, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
