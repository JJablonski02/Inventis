namespace Inventis.Domain.Inventories;

public sealed class InventoryItem : Entity
{
	public Ulid ProductId { get; }
	public string ProductName { get; }
	public decimal Quantity { get; private set; }
	public decimal ExpectedQuantity { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	private InventoryItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	private InventoryItem(
		Ulid productId,
		string productName,
		decimal expectedQuantity)
	{
		ProductId = productId;
		ProductName = productName;
		ExpectedQuantity = expectedQuantity;
	}

	internal static InventoryItem Create(
		Ulid productId,
		string productName,
		decimal expectedQuantity)
		=> new(productId, productName, expectedQuantity);

	internal void IncreaseQuantity()
	{
		if (Quantity + 1 > ExpectedQuantity)
		{
			throw new InvalidOperationException(
				$"Nie można zeskanować produktu '{ProductName}', " +
				$"ponieważ w inwentaryzacji pozostało tylko {ExpectedQuantity - Quantity} szt.");
		}

		Quantity += 1;
	}
}
