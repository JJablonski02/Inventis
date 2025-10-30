using Inventis.Application.Products.Dtos;

namespace Inventis.Domain.Products;

internal static class ProductMapperExtensions
{
	internal static ProductDto ToDto(this Product product)
		=> new(
			product.Id,
			product.Name,
			product.Description,
			product.EanCode,
			product.NetPurchasePrice,
			product.GrossPurchasePrice,
			product.NetSalePrice,
			product.GrossSalePrice,
			product.CurrentTotalQuantity,
			product.CurrentQuantityInStore,
			product.CurrentQuantityInBackroom,
			product.CurrentQuantityInWarehouse,
			product.StoredTotalQuantity,
			product.StoredQuantityInStore,
			product.StoredQuantityInBackroom,
			product.StoredQuantityInWarehouse,
			product.PurchasePriceVatRate,
			product.SalePriceVatRate,
			product.ProviderName,
			product.ProviderContactDetails,
			[.. product.InventoryMovementLogs.OrderByDescending(y => y.CreatedAt).Select(log => log.ToDto())]);

	private static ProductInventoryMovementLogDto ToDto(this ProductInventoryMovementLog log) =>
		new(
			log.ProductId,
			log.ScanId,
			log.Action,
			log.Direction,
			log.CreatedAt,
			log.CurrentQuantityInStoreBefore,
			log.CurrentQuantityInBackroomBefore,
			log.CurrentQuantityInWarehouseBefore,
			log.CurrentQuantityInStoreAfter,
			log.CurrentQuantityInBackroomAfter,
			log.CurrentQuantityInWarehouseAfter,
			log.StoredQuantityInStoreBefore,
			log.StoredQuantityInBackroomBefore,
			log.StoredQuantityInWarehouseBefore,
			log.StoredQuantityInStoreAfter,
			log.StoredQuantityInBackroomAfter,
			log.StoredQuantityInWarehouseAfter
		);
}
