using Inventis.Domain.Inventories.Constants;

namespace Inventis.Application.Inventories.Dtos;

public sealed record InventoryDto(
		Ulid Id,
		Ulid UserId,
		string UserFullName,
		InventoryType Type,
		IReadOnlyCollection<InventoryItemDetailsDto> Items,
		DateTime StartedAt,
		DateTime? CompletedAt);
