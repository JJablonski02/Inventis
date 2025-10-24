using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventis.UI.Models.Inventories;

public sealed class InventorySelectItem(
	string name,
	string value) : ObservableObject
{
	public string Name { get; } = name;
	public string Value { get; } = value;
}
