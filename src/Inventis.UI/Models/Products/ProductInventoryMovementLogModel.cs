using Inventis.Application.Products.Dtos;
using Inventis.Domain.Products;
using Inventis.Domain.Products.Constants;

namespace Inventis.UI.Models.Products;

public record ProductInventoryMovementLogModel
{
	public required string ProductId { get; init; }
	public string? ScanId { get; init; }

	public required string Action { get; init; }
	public required string Direction { get; init; }
	public DateTime CreatedAt { get; init; }

	// Current quantities
	public decimal CurrentQuantityInStoreBefore { get; init; }
	public decimal CurrentQuantityInBackroomBefore { get; init; }
	public decimal CurrentQuantityInWarehouseBefore { get; init; }
	public decimal CurrentQuantityInStoreAfter { get; init; }
	public decimal CurrentQuantityInBackroomAfter { get; init; }
	public decimal CurrentQuantityInWarehouseAfter { get; init; }

	// Totals
	public decimal TotalCurrentQuantityBefore { get; init; }
		= 0;
	public decimal TotalCurrentQuantityAfter { get; init; }
		= 0;

	// Stored quantities
	public decimal StoredQuantityInStoreBefore { get; init; }
	public decimal StoredQuantityInBackroomBefore { get; init; }
	public decimal StoredQuantityInWarehouseBefore { get; init; }
	public decimal StoredQuantityInStoreAfter { get; init; }
	public decimal StoredQuantityInBackroomAfter { get; init; }
	public decimal StoredQuantityInWarehouseAfter { get; init; }

	public static ProductInventoryMovementLogModel FromDto(ProductInventoryMovementLogDto log) =>
		new()
		{
			ProductId = log.ProductId.ToString(),
			ScanId = log.ScanId?.ToString(),
			Action = TranslateAction(log.Action),
			Direction = TranslateDirection(log.Direction),
			CreatedAt = log.CreatedAt,
			CurrentQuantityInStoreBefore = log.CurrentQuantityInStoreBefore,
			CurrentQuantityInBackroomBefore = log.CurrentQuantityInBackroomBefore,
			CurrentQuantityInWarehouseBefore = log.CurrentQuantityInWarehouseBefore,
			CurrentQuantityInStoreAfter = log.CurrentQuantityInStoreAfter,
			CurrentQuantityInBackroomAfter = log.CurrentQuantityInBackroomAfter,
			CurrentQuantityInWarehouseAfter = log.CurrentQuantityInWarehouseAfter,
			TotalCurrentQuantityBefore = log.CurrentQuantityInStoreBefore
										+ log.CurrentQuantityInBackroomBefore
										+ log.CurrentQuantityInWarehouseBefore,
			TotalCurrentQuantityAfter = log.CurrentQuantityInStoreAfter
									   + log.CurrentQuantityInBackroomAfter
									   + log.CurrentQuantityInWarehouseAfter,
			StoredQuantityInStoreBefore = log.StoredQuantityInStoreBefore,
			StoredQuantityInBackroomBefore = log.StoredQuantityInBackroomBefore,
			StoredQuantityInWarehouseBefore = log.StoredQuantityInWarehouseBefore,
			StoredQuantityInStoreAfter = log.StoredQuantityInStoreAfter,
			StoredQuantityInBackroomAfter = log.StoredQuantityInBackroomAfter,
			StoredQuantityInWarehouseAfter = log.StoredQuantityInWarehouseAfter
		};

	private static string TranslateDirection(InventoryMovementLogDirection direction) =>
		direction.Value switch
		{
			nameof(InventoryMovementLogDirection.In) => "Przychód",
			nameof(InventoryMovementLogDirection.Out) => "Rozchód",
			_ => direction.Value
		};

	private static string TranslateAction(InventoryMovementLogAction action) =>
		action.Value switch
		{
			nameof(InventoryMovementLogAction.Scanned) => "Skanowanie",
			nameof(InventoryMovementLogAction.Updated) => "Aktualizacja",
			nameof(InventoryMovementLogAction.Created) => "Utworzenie",
			nameof(InventoryMovementLogAction.ScanDeleted) => "Usunięcie skanu",
			nameof(InventoryMovementLogAction.ClosedReport) => "Zamknięcie raportu",
			_ => action.Value
		};
}
