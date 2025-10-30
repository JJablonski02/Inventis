using Inventis.Application.Exceptions;
using Inventis.Application.Inventories.Dtos;
using Inventis.Domain.Inventories;
using Inventis.Domain.Inventories.Constants;
using Inventis.Domain.Inventories.Dtos;
using Inventis.Domain.Inventories.Repositories;
using Inventis.Domain.Products;
using Inventis.Domain.Products.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Inventis.Application.Inventories.Services;

public interface IInventoryService
{
	Task OpenInventoryAsync(
		InventoryType type,
		Ulid userId,
		string userFullName,
		CancellationToken cancellationToken);

	Task<InventoryDto> GetInventoryAsync(CancellationToken cancellationToken);

	Task AddScannedProductAsync(string eanCode, CancellationToken cancellationToken);

	Task CloseInventoryAsync(
		CancellationToken cancellationToken);
}

internal sealed class InventoryService(IServiceProvider serviceProvider) : ScopedServiceBase(serviceProvider), IInventoryService
{
	public Task OpenInventoryAsync(
		InventoryType type,
		Ulid userId,
		string userFullName,
		CancellationToken cancellationToken)
		=> UseScopeAsync(async (sc) =>
		{
			var inventoriesRepository = sc.GetRequiredService<IReadWriteInventoriesRepository>();

			if (await inventoriesRepository.AnyAsync(inventory => !inventory.IsCompleted, cancellationToken))
			{
				throw new InvalidOperationException("Rozpoczęcie nowej inwentaryazcji nie jest możliwe jeżeli inna inwentaryzacja jest już rozpoczęta.");
			}

			IReadOnlyCollection<InventoryItemDto> items = [.. await PrepareInventoryItems(sc, type, cancellationToken)];

			if (items.Count is 0)
			{
				throw new InvalidOperationException(
					"Nie można rozpocząć inwentaryzacji – lista produktów jest pusta. Dodaj produkty, aby rozpocząć proces.");
			}

			if (items.All(item => item.ExpectedQuantity <= 0))
			{
				throw new InvalidOperationException(
					"Nie można rozpocząć inwentaryzacji – wszystkie produkty mają stan magazynowy równy 0. Sprawdź stany magazynowe przed rozpoczęciem.");
			}

			var inventory = Inventory.Create(userId, userFullName, type, [.. items]);

			await inventoriesRepository.AddAndSaveChangesAsync(inventory, cancellationToken);
		});

	public Task CloseInventoryAsync(CancellationToken cancellationToken)
		=> UseScopeAsync(async (sc) =>
		{
			var inventoriesRepository = sc.GetRequiredService<IReadWriteInventoriesRepository>();
			var inventory = await inventoriesRepository.SingleOrDefaultAsync(
				y => !y.IsCompleted,
				cancellationToken);

			if (inventory is null)
			{
				throw new InvalidOperationException("Zamknięcie inwentaryzacji nie jest możliwe, ponieważ żadna inwentaryzacja nie jest aktualnie otwarta.");
			}

			inventory.Complete();

			await inventoriesRepository.SaveChangesAsync(cancellationToken);
		});

	public Task AddScannedProductAsync(string eanCode, CancellationToken cancellationToken)
		=> UseScopeAsync(async (sc) =>
		{
			var inventoriesRepository = sc.GetRequiredService<IReadWriteInventoriesRepository>();

			var inventory = await inventoriesRepository.SingleOrDefaultAsync(
				y => !y.IsCompleted,
				cancellationToken);

			if (inventory is null)
			{
				throw new InvalidOperationException("Dodanie zeskanowanego produktu nie jest możliwe, ponieważ żadna inwentaryzacja nie jest aktualnie otwarta.");
			}

			var productsRepository = sc.GetRequiredService<IReadProductRepository>();

			var product = (await productsRepository.WhereAsync(
				y => y.EanCode == eanCode,
				cancellationToken))
				.SingleOrDefault()
				?? throw new NotFoundException("Product");

			inventory.AddScannedProduct(product.Id);

			await inventoriesRepository.SaveChangesAsync(cancellationToken);
		});

	private static async Task<IEnumerable<InventoryItemDto>> PrepareInventoryItems(
		IServiceProvider sc,
		InventoryType type,
		CancellationToken cancellationToken)
	{
		var productsRepository = sc.GetRequiredService<IReadProductRepository>();

		List<Product> products;

		if (type == InventoryType.Total)
		{
			products = [.. (await productsRepository.WhereAsync(
				y => y.StoredQuantityInBackroom > 0 ||
					 y.StoredQuantityInStore > 0 ||
					 y.StoredQuantityInWarehouse > 0,
				cancellationToken))];
		}
		else if (type == InventoryType.StoreAndBackroom)
		{
			products = [.. (await productsRepository.WhereAsync(
				y => y.StoredQuantityInStore > 0 ||
					 y.StoredQuantityInBackroom > 0,
				cancellationToken))];
		}
		else if (type == InventoryType.Store)
		{
			products = [.. (await productsRepository.WhereAsync(
				y => y.StoredQuantityInStore > 0,
				cancellationToken))];
		}
		else if (type == InventoryType.Backroom)
		{
			products = [.. (await productsRepository.WhereAsync(
				y => y.StoredQuantityInBackroom > 0,
				cancellationToken))];
		}
		else if (type == InventoryType.Warehouse)
		{
			products = [.. (await productsRepository.WhereAsync(
				y => y.StoredQuantityInWarehouse > 0,
				cancellationToken))];
		}
		else
		{
			throw new InvalidOperationException($"Unknown inventory type: {type.Value}");
		}

		IEnumerable<InventoryItemDto> items;

		if (type == InventoryType.Store)
		{
			items = products.Select(y => new InventoryItemDto(y.Id, y.Name, y.StoredQuantityInStore));
		}
		else if (type == InventoryType.Backroom)
		{
			items = products.Select(y => new InventoryItemDto(y.Id, y.Name, y.StoredQuantityInBackroom));
		}
		else if (type == InventoryType.Warehouse)
		{
			items = products.Select(y => new InventoryItemDto(y.Id, y.Name, y.StoredQuantityInWarehouse));
		}
		else if (type == InventoryType.StoreAndBackroom)
		{
			items = products.Select(y => new InventoryItemDto(
				y.Id,
				y.Name,
				y.StoredQuantityInStore + y.StoredQuantityInBackroom));
		}
		else
		{
			items = products
				.OrderByDescending(y => y.CreatedAt)
				.Select(y => new InventoryItemDto(
				y.Id,
				y.Name,
				y.StoredQuantityInStore + y.StoredQuantityInBackroom + y.StoredQuantityInWarehouse));
		}

		return items;
	}

	public Task<InventoryDto> GetInventoryAsync(CancellationToken cancellationToken)
		=> UseScopeAsync<InventoryDto>(async (sc) =>
		{
			var inventoryRepository = sc.GetRequiredService<IReadInventoriesRepository>();

			var inventory = await inventoryRepository.SingleOrDefaultAsync(y => !y.IsCompleted, cancellationToken);

			return new InventoryDto(
				inventory.Id,
				inventory.UserId,
				inventory.UserFullName,
				inventory.Type,
				[.. inventory.Items.Select(y => new InventoryItemDetailsDto(
					y.Id,
					y.ProductId,
					y.ProductName,
					y.ExpectedQuantity,
					y.Quantity))],
				inventory.StartedAt,
				inventory.CompletedAt);
		});
}
