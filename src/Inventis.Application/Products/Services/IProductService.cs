using Inventis.Application.Products.Dtos.Requests;
using Inventis.Domain.Products;
using Inventis.Domain.Products.Repositories;
using Inventis.Domain.Products.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.Application.Products.Services;

public interface IProductService
{
	string GenerateEan13();

	Task CreateAsync(CreateProductRequestDto request, CancellationToken cancellationToken);
}

internal sealed class ProductService(
	IServiceProvider serviceProvider) : ScopedServiceBase(serviceProvider), IProductService
{
	public string GenerateEan13()
		=> Ean13Generator.GenerateRandom();

	public Task CreateAsync(CreateProductRequestDto request, CancellationToken cancellationToken)
		=> UseScopeAsync(async (scope) =>
		{
			var productRepository = scope.GetRequiredService<IReadWriteProductRepository>();

			if (await productRepository.AnyAsync(product => product.Name == request.Name, cancellationToken))
			{
				throw new InvalidOperationException($"Produkt o nazwie '{request.Name}' już istnieje.");
			}

			var product = Product.Create(
				request.Name,
				request.Description,
				request.EanCode,
				request.NetPurchasePrice,
				request.GrossPurchasePrice,
				request.NetSalePrice,
				request.GrossSalePrice,
				request.QuantityInStore,
				request.QuantityInBackroom,
				request.QuantityInWarehouse,
				request.VatRate,
				request.ProviderName,
				request.ProviderContactDetails);

			await productRepository.AddAndSaveChangesAsync(product, cancellationToken);
		});
}
