namespace Inventis.Application.DailyInventoryReports.Dtos;

/// <summary>
/// Daily inventory report data transfer object
/// </summary>
/// <param name="Id">Identifier of this daily inventory report</param>
/// <param name="CreatedAt">Determines date when this daily inventory report has been created</param>
/// <param name="ClosedAt">Determines time when this daily report has been closed</param>
public sealed record DailyInventoryReportDto(
	Ulid Id,
	DateTime CreatedAt,
	DateTime? ClosedAt,
	IReadOnlyCollection<DailyInventoryScanDto> Scans);
