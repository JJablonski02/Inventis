namespace Inventis.Domain.Products.Constants;

public sealed record InventoryMovementLogAction
{
	public InventoryMovementLogAction(string value)
	{
		Value = value;
	}

	public string Value { get; }

	public static InventoryMovementLogAction Scanned => new("Scanned");
	public static InventoryMovementLogAction Updated => new("Updated");
	public static InventoryMovementLogAction Created => new("Created");
	public static InventoryMovementLogAction ScanDeleted => new("ScanDeleted");
	public static InventoryMovementLogAction ClosedReport => new("ClosedReport");

	public static InventoryMovementLogAction Parse(string value)
		=> value switch
		{
			nameof(Scanned) => Scanned,
			nameof(Updated) => Updated,
			nameof(Created) => Created,
			nameof(ScanDeleted) => ScanDeleted,
			nameof(ClosedReport) => ClosedReport,
			_ => throw new ArgumentException($"Invalid inventory movement log action: {value}")
		};
}
