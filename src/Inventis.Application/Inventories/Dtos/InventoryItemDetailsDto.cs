namespace Inventis.Application.Inventories.Dtos;

public sealed record InventoryItemDetailsDto(
		Ulid Id,
		Ulid ProductId,
		string ProductName,
		decimal ExpectedQuantity,
		decimal Quantity);
