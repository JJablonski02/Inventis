namespace Inventis.Application.DailyInventoryReports.Dtos;

public sealed record DailyInventoryScanDto(
	Ulid Id,
	Ulid ProductId,
	string ProductName,
	decimal GrossSalePrice,
	bool IsDeleted,
	DateTime ScanTime);
