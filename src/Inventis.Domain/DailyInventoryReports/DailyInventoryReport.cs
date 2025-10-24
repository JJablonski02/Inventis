namespace Inventis.Domain.DailyInventoryReports;

public sealed class DailyInventoryReport : Entity
{
	private readonly List<DailyInventoryScan> _dailyScans = [];

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	private DailyInventoryReport() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	private DailyInventoryReport(
		DateTime createdAt)
	{
		IsClosed = false;
		ClosedAt = null;
		CreatedAt = createdAt;
	}

	public bool IsClosed { get; private set; }
	public DateTime CreatedAt { get; }
	public DateTime? ClosedAt { get; private set; }

	public IReadOnlyCollection<DailyInventoryScan> DailyScans => _dailyScans.AsReadOnly();

	public static DailyInventoryReport Create()
		=> new(DateTime.Now);

	public void AddScan(Ulid productId)
	{
		_dailyScans.Add(DailyInventoryScan.Create(
				productId));
	}

	public void DeleteScan(Ulid scanId)
	{
		var scanToDelete = _dailyScans.SingleOrDefault(y => y.Id == scanId)
			?? throw new DomainException(
				"DailyInventoryReport.MissingScan",
				"Provided scan identifier does not exist.");

		scanToDelete.Delete();
	}

	public void CloseReport()
	{
		if (IsClosed)
		{
			return;
		}

		IsClosed = true;
		ClosedAt = DateTime.Now;
	}
}
