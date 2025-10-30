namespace Inventis.Application.Products.Dtos;

public sealed record ProductDto(
	Ulid Id,
	string Name,
	string? Description,
	string EanCode,
	decimal NetPurchasePrice,
	decimal GrossPurchasePrice,
	decimal NetSalePrice,
	decimal GrossSalePrice,
	decimal CurrentTotalQuantity,
	decimal CurrentQuantityInStore,
	decimal CurrentQuantityInBackroom,
	decimal CurrentQuantityInWarehouse,
	decimal StoredTotalQuantity,
	decimal StoredQuantityInStore,
	decimal StoredQuantityInBackroom,
	decimal StoredQuantityInWarehouse,
	decimal PurchasePriceVatRate,
	decimal SalePriceVatRate,
	string? ProviderName,
	string? ProviderContactDetails,
	IReadOnlyCollection<ProductInventoryMovementLogDto> Logs);
