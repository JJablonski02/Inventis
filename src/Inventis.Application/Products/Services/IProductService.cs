using Inventis.Application.Products.Dtos;
using Inventis.Application.Products.Dtos.Requests;
using Inventis.Domain.Products;
using Inventis.Domain.Products.Constants;
using Inventis.Domain.Products.Repositories;
using Inventis.Domain.Products.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.Application.Products.Services;

public interface IProductService
{
	string GenerateEan13();

	Task<ProductDto> GetByIdAsync(Ulid productId, CancellationToken cancellationToken);
	Task<IReadOnlyCollection<ProductDto>> GetAllAsync(CancellationToken cancellationToken);

	Task CreateAsync(CreateProductRequestDto request, CancellationToken cancellationToken);

	Task UpdateAsync(UpdateProductRequestDto request, CancellationToken cancellationToken);

	Task DecreaseSingleQuantityAsync(Ulid productId, QuantityType quantityType, CancellationToken cancellationToken);
}

internal sealed class ProductService(
	IServiceProvider serviceProvider) : ScopedServiceBase(serviceProvider), IProductService
{
	public string GenerateEan13()
		=> Ean13Generator.GenerateRandom();

	public async Task<IReadOnlyCollection<ProductDto>> GetAllAsync(CancellationToken cancellationToken)
		=> await UseScopeAsync(async (scope) =>
		{
			var productRepository = scope.GetRequiredService<IReadProductRepository>();
			var products = await productRepository.GetAllAsync(cancellationToken);
			return products
			.OrderByDescending(y => y.CreatedAt)
			.Select(x => x.ToDto()).ToList();
		});

	public Task CreateAsync(CreateProductRequestDto request, CancellationToken cancellationToken)
		=> UseScopeAsync(async (scope) =>
		{
			var productRepository = scope.GetRequiredService<IReadWriteProductRepository>();

			if (await productRepository.AnyAsync(product => product.Name == request.Name, cancellationToken))
			{
				throw new InvalidOperationException($"Produkt o nazwie '{request.Name}' już istnieje.");
			}

			if (await productRepository.AnyAsync(product => product.EanCode == request.EanCode, cancellationToken))
			{
				throw new InvalidOperationException($"Produkt z kodem EAN '{request.EanCode}' już istnieje.");
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

	public Task UpdateAsync(UpdateProductRequestDto request, CancellationToken cancellationToken)
		=> UseScopeAsync(async (scope) =>
		{
			var productRepository = scope.GetRequiredService<IReadWriteProductRepository>();

			var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);

			if (await productRepository.AnyAsync(p => p.Name == request.Name && p.Id != request.ProductId, cancellationToken))
			{
				throw new InvalidOperationException($"Produkt o nazwie '{request.Name}' już istnieje.");
			}

			product.Update(
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

			await productRepository.SaveChangesAsync(cancellationToken);
		});

	public Task<ProductDto> GetByIdAsync(
		Ulid productId,
		CancellationToken cancellationToken)
		=> UseScopeAsync(async (scope) =>
		{
			var productRepository = scope.GetRequiredService<IReadWriteProductRepository>();

			var product = await productRepository.GetByIdAsync(productId, cancellationToken);

			return product.ToDto();
		});

	public Task DecreaseSingleQuantityAsync(
		Ulid productId,
		QuantityType quantityType,
		CancellationToken cancellationToken)
		=> UseScopeAsync(async (scope) =>
		{
			var productRepository = scope.GetRequiredService<IReadWriteProductRepository>();

			var product = await productRepository.GetByIdAsync(productId, cancellationToken);

			product.DecreaseSingleQuantity(quantityType);

			await productRepository.SaveChangesAsync(cancellationToken);
		});
}
