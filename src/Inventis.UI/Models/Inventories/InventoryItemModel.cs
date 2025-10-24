using CommunityToolkit.Mvvm.ComponentModel;
using Inventis.Application.Inventories.Dtos;

namespace Inventis.UI.Models.Inventories;

internal sealed partial class InventoryItemModel : ObservableObject
{
	[ObservableProperty]
	private string _id;

	[ObservableProperty]
	private string _productId;

	[ObservableProperty]
	private string _productName;

	[ObservableProperty]
	private decimal _expectedQuantity;

	[ObservableProperty]
	private decimal _quantity;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	public InventoryItemModel() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	internal InventoryItemModel(
		string id,
		string productId,
		string productName,
		decimal expectedQuantity,
		decimal quantity)
	{
		_id = id;
		_productId = productId;
		_productName = productName;
		_expectedQuantity = expectedQuantity;
		_quantity = quantity;
	}

	internal static InventoryItemModel Create(InventoryItemDetailsDto dto)
	{
		return new InventoryItemModel(
			dto.Id.ToString(),
			dto.ProductId.ToString(),
			dto.ProductName,
			dto.ExpectedQuantity,
			dto.Quantity
		);
	}
}
