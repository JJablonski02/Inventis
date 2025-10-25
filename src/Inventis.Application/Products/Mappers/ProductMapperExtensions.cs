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
			product.TotalQuantity,
			product.QuantityInStore,
			product.QuantityInBackroom,
			product.QuantityInWarehouse,
			product.VatRate,
			product.ProviderName,
			product.ProviderContactDetails);
}
