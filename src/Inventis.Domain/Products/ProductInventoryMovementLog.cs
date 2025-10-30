using Inventis.Domain.Products.Constants;

namespace Inventis.Domain.Products;

public sealed class ProductInventoryMovementLog : Entity
{
	public ProductInventoryMovementLog(
		Ulid productId,
		Ulid? scanId,
		QuantityType? quantityType,
		InventoryMovementLogAction action,
		InventoryMovementLogDirection direction,
		decimal currentQuantityInStoreBefore,
		decimal currentQuantityInBackroomBefore,
		decimal currentQuantityInWarehouseBefore,
		decimal currentQuantityInStoreAfter,
		decimal currentQuantityInBackroomAfter,
		decimal currentQuantityInWarehouseAfter,
		decimal storedQuantityInStoreBefore,
		decimal storedQuantityInBackroomBefore,
		decimal storedQuantityInWarehouseBefore,
		decimal storedQuantityInStoreAfter,
		decimal storedQuantityInBackroomAfter,
		decimal storedQuantityInWarehouseAfter)
	{
		ProductId = productId;
		ScanId = scanId;
		QuantityType = quantityType;
		Action = action;
		Direction = direction;

		CurrentQuantityInStoreBefore = currentQuantityInStoreBefore;
		CurrentQuantityInBackroomBefore = currentQuantityInBackroomBefore;
		CurrentQuantityInWarehouseBefore = currentQuantityInWarehouseBefore;
		CurrentQuantityInStoreAfter = currentQuantityInStoreAfter;
		CurrentQuantityInBackroomAfter = currentQuantityInBackroomAfter;
		CurrentQuantityInWarehouseAfter = currentQuantityInWarehouseAfter;

		StoredQuantityInStoreBefore = storedQuantityInStoreBefore;
		StoredQuantityInBackroomBefore = storedQuantityInBackroomBefore;
		StoredQuantityInWarehouseBefore = storedQuantityInWarehouseBefore;
		StoredQuantityInStoreAfter = storedQuantityInStoreAfter;
		StoredQuantityInBackroomAfter = storedQuantityInBackroomAfter;
		StoredQuantityInWarehouseAfter = storedQuantityInWarehouseAfter;
		CreatedAt = DateTime.Now;
	}


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	private ProductInventoryMovementLog() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	public Ulid ProductId { get; }
	public Product? Product { get; }

	public Ulid? ScanId { get; }
	public QuantityType? QuantityType { get; }
	public InventoryMovementLogAction Action { get; }
	public InventoryMovementLogDirection Direction { get; }
	public DateTime CreatedAt { get; }

	// Current quantities
	public decimal CurrentQuantityInStoreBefore { get; }
	public decimal CurrentQuantityInBackroomBefore { get; }
	public decimal CurrentQuantityInWarehouseBefore { get; }
	public decimal CurrentQuantityInStoreAfter { get; }
	public decimal CurrentQuantityInBackroomAfter { get; }
	public decimal CurrentQuantityInWarehouseAfter { get; }

	public decimal TotalCurrentQuantityAfter => CurrentQuantityInStoreAfter + CurrentQuantityInBackroomAfter + CurrentQuantityInWarehouseAfter;
	public decimal TotalStoredQuantityAfter => StoredQuantityInStoreAfter + StoredQuantityInBackroomAfter + StoredQuantityInWarehouseAfter;

	// Stored quantities
	public decimal StoredQuantityInStoreBefore { get; }
	public decimal StoredQuantityInBackroomBefore { get; }
	public decimal StoredQuantityInWarehouseBefore { get; }
	public decimal StoredQuantityInStoreAfter { get; }
	public decimal StoredQuantityInBackroomAfter { get; }
	public decimal StoredQuantityInWarehouseAfter { get; }

	internal static ProductInventoryMovementLog Create(
		Ulid productId,
		Ulid? scanId,
		QuantityType? quantityType,
		InventoryMovementLogAction action,
		InventoryMovementLogDirection direction,
		decimal currentQuantityInStoreBefore,
		decimal currentQuantityInBackroomBefore,
		decimal currentQuantityInWarehouseBefore,
		decimal currentQuantityInStoreAfter,
		decimal currentQuantityInBackroomAfter,
		decimal currentQuantityInWarehouseAfter,
		decimal storedQuantityInStoreBefore,
		decimal storedQuantityInBackroomBefore,
		decimal storedQuantityInWarehouseBefore,
		decimal storedQuantityInStoreAfter,
		decimal storedQuantityInBackroomAfter,
		decimal storedQuantityInWarehouseAfter)
	{
		return new ProductInventoryMovementLog(
			productId,
			scanId,
			quantityType,
			action,
			direction,
			currentQuantityInStoreBefore,
			currentQuantityInBackroomBefore,
			currentQuantityInWarehouseBefore,
			currentQuantityInStoreAfter,
			currentQuantityInBackroomAfter,
			currentQuantityInWarehouseAfter,
			storedQuantityInStoreBefore,
			storedQuantityInBackroomBefore,
			storedQuantityInWarehouseBefore,
			storedQuantityInStoreAfter,
			storedQuantityInBackroomAfter,
			storedQuantityInWarehouseAfter
		);
	}
}
