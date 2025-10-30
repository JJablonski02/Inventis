namespace Inventis.Domain.Products.Constants;

public sealed record InventoryMovementLogDirection
{
	public InventoryMovementLogDirection(string value)
	{
		Value = value;
	}

	public string Value { get; }

	public static InventoryMovementLogDirection In => new("In");
	public static InventoryMovementLogDirection Out => new("Out");

	public static InventoryMovementLogDirection Parse(string value)
		=> value switch
		{
			nameof(In) => In,
			nameof(Out) => Out,
			_ => throw new ArgumentException($"Invalid inventory movement log direction: {value}")
		};
}
