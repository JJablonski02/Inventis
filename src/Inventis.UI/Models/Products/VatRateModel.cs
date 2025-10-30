using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventis.UI.Models.Products;

internal sealed partial class VatRateModel : ObservableObject
{
	[ObservableProperty]
	private string _name;

	[ObservableProperty]
	private decimal _value;

	public VatRateModel(
		string name,
		decimal value)
	{
		Name = name;
		Value = value;
	}
}
