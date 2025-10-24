namespace Inventis.Domain.Inventories.Dtos;

public sealed record InventoryItemDto(
	Ulid ProductId,
	string ProductName,
	decimal ExpectedQuantity);
