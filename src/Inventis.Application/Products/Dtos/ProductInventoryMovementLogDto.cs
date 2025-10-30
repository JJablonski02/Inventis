using Inventis.Domain.Products.Constants;
namespace Inventis.Application.Products.Dtos;

public record ProductInventoryMovementLogDto(
	Ulid ProductId,
	Ulid? ScanId,
	InventoryMovementLogAction Action,
	InventoryMovementLogDirection Direction,
	DateTime CreatedAt,

	// Current quantities
	decimal CurrentQuantityInStoreBefore,
	decimal CurrentQuantityInBackroomBefore,
	decimal CurrentQuantityInWarehouseBefore,
	decimal CurrentQuantityInStoreAfter,
	decimal CurrentQuantityInBackroomAfter,
	decimal CurrentQuantityInWarehouseAfter,

	// Stored quantities
	decimal StoredQuantityInStoreBefore,
	decimal StoredQuantityInBackroomBefore,
	decimal StoredQuantityInWarehouseBefore,
	decimal StoredQuantityInStoreAfter,
	decimal StoredQuantityInBackroomAfter,
	decimal StoredQuantityInWarehouseAfter
);
