using System.Collections.ObjectModel;
using Inventis.Application.DailyInventoryReports.Dtos;

namespace Inventis.UI.ViewModels;

internal sealed partial class SalesViewModel : ViewModelBase
{
	public ObservableCollection<DailyInventoryScanDto> DailyInventoryScans { get; } = new()
	{
		new DailyInventoryScanDto(Ulid.NewUlid(), "Coca-Cola 0.5L", 4.50m, DateTime.Now.AddMinutes(-15)),
		new DailyInventoryScanDto(Ulid.NewUlid(), "Pepsi 1L", 5.20m, DateTime.Now.AddMinutes(-30)),
		new DailyInventoryScanDto(Ulid.NewUlid(), "Sprite 0.5L", 4.30m, DateTime.Now.AddMinutes(-60))
	};
}
