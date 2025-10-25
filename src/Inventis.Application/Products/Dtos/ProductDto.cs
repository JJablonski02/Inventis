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
	decimal TotalQuantity,
	decimal QuantityInStore,
	decimal QuantityInBackroom,
	decimal QuantityInWarehouse,
	decimal VatRate,
	string? ProviderName,
	string? ProviderContactDetails);
