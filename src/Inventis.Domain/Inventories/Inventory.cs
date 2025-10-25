using Inventis.Domain.Inventories.Constants;
using Inventis.Domain.Inventories.Dtos;

namespace Inventis.Domain.Inventories;

public sealed class Inventory : Entity
{
	private readonly List<InventoryItem> _items = [];

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	private Inventory() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	private Inventory(
		Ulid userId,
		string userFullName,
		InventoryType type,
		IReadOnlyCollection<InventoryItem> items,
		DateTime startedAt)
	{
		UserId = userId;
		UserFullName = userFullName;
		Type = type;
		_items = [.. items];
		StartedAt = startedAt;
	}
	public Ulid UserId { get; }
	public string UserFullName { get; }
	public bool IsCompleted { get; private set; }
	public DateTime StartedAt { get; }
	public DateTime? CompletedAt { get; private set; }
	public InventoryType Type { get; }
	public IReadOnlyCollection<InventoryItem> Items => _items.AsReadOnly();

	public static Inventory Create(
		Ulid userId,
		string userFullName,
		InventoryType type,
		IReadOnlyCollection<InventoryItemDto> items)
	{
		var inventoryItems = items.Select(item =>
			InventoryItem.Create(
				item.ProductId,
				item.ProductName,
				item.ExpectedQuantity))
			.ToList();

		return new Inventory(
			userId,
			userFullName,
			type,
			inventoryItems,
			DateTime.Now);
	}

	public void AddScannedProduct(
		Ulid productId)
	{
		var item = Items.SingleOrDefault(prod => prod.ProductId == productId)
			?? throw new ArgumentException("Produkt nie należy do inwentaryzacji.");

		item.IncreaseQuantity();
	}

	public void Complete()
	{
		if (IsCompleted)
		{
			return;
		}

		IsCompleted = true;
		CompletedAt = DateTime.Now;
	}
}
