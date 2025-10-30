namespace Inventis.Application.Products.Dtos.Requests;

public sealed record UpdateProductRequestDto(
	Ulid ProductId,
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
	decimal PurchasePriceVatRate,
	decimal SalePriceVatRate,
	string? ProviderName,
	string? ProviderContactDetails);
