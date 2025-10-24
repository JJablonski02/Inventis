namespace Inventis.Domain.Inventories.Constants;

public sealed record InventoryType
{
	private InventoryType(string value)
	{
		Value = value;
	}

	public string Value { get; }

	public static InventoryType Total => new("Total");
	public static InventoryType StoreAndBackroom => new("StoreAndBackroom");
	public static InventoryType Store => new("Store");
	public static InventoryType Backroom => new("Backroom");
	public static InventoryType Warehouse => new("Warehouse");

	public static InventoryType Parse(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ArgumentException("Inventory type cannot be empty.");
		}

		return value switch
		{
			var v when string.Equals(v, Total.Value, StringComparison.OrdinalIgnoreCase) => Total,
			var v when string.Equals(v, StoreAndBackroom.Value, StringComparison.OrdinalIgnoreCase) => StoreAndBackroom,
			var v when string.Equals(v, Store.Value, StringComparison.OrdinalIgnoreCase) => Store,
			var v when string.Equals(v, Backroom.Value, StringComparison.OrdinalIgnoreCase) => Backroom,
			var v when string.Equals(v, Warehouse.Value, StringComparison.OrdinalIgnoreCase) => Warehouse,
			_ => throw new ArgumentException("Invalid inventory type")
		};
	}
}
