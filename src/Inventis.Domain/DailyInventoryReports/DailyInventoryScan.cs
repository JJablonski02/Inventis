namespace Inventis.Domain.DailyInventoryReports;

/// <summary>
/// Daily inventory scan entity
/// </summary>
public sealed class DailyInventoryScan : Entity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	private DailyInventoryScan() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	private DailyInventoryScan(
		Ulid productId,
		bool isDeleted,
		string? note,
		DateTime scanTime)
	{
		ProductId = productId;
		IsDeleted = isDeleted;
		Note = note;
		ScanTime = scanTime;
	}

	public Ulid ProductId { get; }
	public bool IsDeleted { get; private set; }
	public string? Note { get; }
	public DateTime ScanTime { get; }

	internal static DailyInventoryScan Create(
		Ulid productId)
	{
		return new(
			productId,
			isDeleted: false,
			note: null,
			DateTime.Now);
	}

	internal void Delete()
	{
		if (IsDeleted)
		{
			return;
		}

		IsDeleted = true;
	}
}
