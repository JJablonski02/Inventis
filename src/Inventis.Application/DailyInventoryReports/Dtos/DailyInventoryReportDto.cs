namespace Inventis.Application.DailyInventoryReports.Dtos;

/// <summary>
/// Daily inventory report data transfer object
/// </summary>
/// <param name="Id">Identifier of this daily inventory report</param>
/// <param name="CreatedAt">Determines date when this daily inventory report has been created</param>
public sealed record DailyInventoryReportDto(
	Ulid Id,
	DateTime CreatedAt,
	IReadOnlyCollection<DailyInventoryScanDto> Scans);
