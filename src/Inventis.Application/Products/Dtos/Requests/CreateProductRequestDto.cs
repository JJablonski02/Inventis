namespace Inventis.Application.Products.Dtos.Requests;

public sealed record CreateProductRequestDto(
	string Name,
	string? Description,
	string EanCode,
	decimal NetPurchasePrice,
	decimal GrossPurchasePrice,
	decimal NetSalePrice,
	decimal GrossSalePrice,
	decimal QuantityInStore,
	decimal QuantityInBackroom,
	decimal QuantityInWarehouse,
	decimal VatRate,
	string? ProviderName,
	string? ProviderContactDetails);
