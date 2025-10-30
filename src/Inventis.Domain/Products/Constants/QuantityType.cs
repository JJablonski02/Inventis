namespace Inventis.Domain.Products.Constants;

public sealed record QuantityType
{
	public QuantityType(string value)
	{
			Value = value;
	}

	public string Value { get; }

	public static QuantityType InStore => new("InStore");
	public static QuantityType InBackroom => new("InBackroom");
	public static QuantityType InWarehouse => new("InWarehouse");

	public static QuantityType Parse(string value)
		=> value switch
		{
			nameof(InStore) => InStore,
			nameof(InBackroom) => InBackroom,
			nameof(InWarehouse) => InWarehouse,
			_ => throw new ArgumentException("Invalid quantity type")
		};
}
