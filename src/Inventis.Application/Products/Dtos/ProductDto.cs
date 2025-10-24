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
	decimal TotalPurchaseGrossValue,
	decimal TotalSaleGrossValue,
	decimal Quantity,
	decimal VatRate,
	string? ProviderName,
	string? ProviderContactDetails);
