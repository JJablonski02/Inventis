namespace Inventis.Application.DailyInventoryReports.Dtos;

public sealed record DailyInventoryScanDto(
	Ulid Id,
	string ProductName,
	decimal GrossSalePrice,
	DateTime ScanTime);
