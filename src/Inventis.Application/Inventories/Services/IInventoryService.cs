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
				y => y.QuantityInBackroom > 0 ||
					 y.QuantityInStore > 0 ||
					 y.QuantityInWarehouse > 0,
				cancellationToken))];
		}
		else if (type == InventoryType.StoreAndBackroom)
		{
			products = [.. (await productsRepository.WhereAsync(
				y => y.QuantityInStore > 0 ||
					 y.QuantityInBackroom > 0,
				cancellationToken))];
		}
		else if (type == InventoryType.Store)
		{
			products = [.. (await productsRepository.WhereAsync(
				y => y.QuantityInStore > 0,
				cancellationToken))];
		}
		else if (type == InventoryType.Backroom)
		{
			products = [.. (await productsRepository.WhereAsync(
				y => y.QuantityInBackroom > 0,
				cancellationToken))];
		}
		else if (type == InventoryType.Warehouse)
		{
			products = [.. (await productsRepository.WhereAsync(
				y => y.QuantityInWarehouse > 0,
				cancellationToken))];
		}
		else
		{
			throw new InvalidOperationException($"Unknown inventory type: {type.Value}");
		}

		IEnumerable<InventoryItemDto> items;

		if (type == InventoryType.Store)
		{
			items = products.Select(y => new InventoryItemDto(y.Id, y.Name, y.QuantityInStore));
		}
		else if (type == InventoryType.Backroom)
		{
			items = products.Select(y => new InventoryItemDto(y.Id, y.Name, y.QuantityInBackroom));
		}
		else if (type == InventoryType.Warehouse)
		{
			items = products.Select(y => new InventoryItemDto(y.Id, y.Name, y.QuantityInWarehouse));
		}
		else if (type == InventoryType.StoreAndBackroom)
		{
			items = products.Select(y => new InventoryItemDto(
				y.Id,
				y.Name,
				y.QuantityInStore + y.QuantityInBackroom));
		}
		else
		{
			items = products.Select(y => new InventoryItemDto(
				y.Id,
				y.Name,
				y.QuantityInStore + y.QuantityInBackroom + y.QuantityInWarehouse));
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
