using CommunityToolkit.Mvvm.ComponentModel;
using Inventis.Application.Inventories.Dtos;

namespace Inventis.UI.Models.Inventories;

internal sealed partial class InventoryModel : ObservableObject
{
	[ObservableProperty]
	private string _id;

	[ObservableProperty]
	private string _userId;

	[ObservableProperty]
	private string _userFullName;

	[ObservableProperty]
	private string _type;

	[ObservableProperty]
	private IReadOnlyCollection<InventoryItemModel> _items;

	[ObservableProperty]
	private string _startedAt;

	[ObservableProperty]
	private string _completedAt;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	internal InventoryModel() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	internal InventoryModel(
		string id,
		string userId,
		string userFullName,
		string type,
		IReadOnlyCollection<InventoryItemModel> items,
		string startedAt,
		string completedAt)
	{
		_id = id;
		_userId = userId;
		_userFullName = userFullName;
		_type = type;
		_items = items;
		_startedAt = startedAt;
		_completedAt = completedAt;
	}

	// Fabryka z DTO
	internal static InventoryModel Create(InventoryDto dto)
	{
		var items = dto.Items
			.Select(x => new InventoryItemModel(
				x.Id.ToString(),
				x.ProductId.ToString(),
				x.ProductName,
				x.ExpectedQuantity,
				x.Quantity))
			.ToList()
			.AsReadOnly();

		return new InventoryModel(
			dto.Id.ToString(),
			dto.UserId.ToString(),
			dto.UserFullName,
			dto.Type.Value.ToString(),
			items,
			dto.StartedAt.ToString(),
			dto.CompletedAt?.ToString() ?? string.Empty
		);
	}
}
