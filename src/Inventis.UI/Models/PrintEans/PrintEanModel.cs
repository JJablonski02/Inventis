using CommunityToolkit.Mvvm.ComponentModel;
using Inventis.Application.Products.Dtos;

namespace Inventis.UI.Models.PrintEans;

internal sealed partial class PrintEanModel : ObservableObject
{
	public required Ulid Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public required string EanCode { get; set; }
	public decimal NetPurchasePrice { get; set; }
	public decimal GrossPurchasePrice { get; set; }
	public decimal NetSalePrice { get; set; }
	public decimal GrossSalePrice { get; set; }
	public decimal TotalQuantity { get; set; }
	public decimal QuantityInStore { get; set; }
	public decimal QuantityInBackroom { get; set; }
	public decimal QuantityInWarehouse { get; set; }
	public decimal VatRate { get; set; }
	public string? ProviderName { get; set; }
	public string? ProviderContactDetails { get; set; }


	public bool PrintFromShop { get; set; } = true;
	public bool PrintFromBackroom { get; set; } = true;
	public bool PrintFromWarehouse { get; set; } = true;

	[ObservableProperty]
	private int _labelCount;
	public bool IsSelected { get; set; } = false;

	public static PrintEanModel ToPrintEanModel(ProductDto product, bool isSelected)
	{
		return new PrintEanModel
		{
			Id = product.Id,
			Name = product.Name,
			Description = product.Description,
			EanCode = product.EanCode,
			NetPurchasePrice = product.NetPurchasePrice,
			GrossPurchasePrice = product.GrossPurchasePrice,
			NetSalePrice = product.NetSalePrice,
			GrossSalePrice = product.GrossSalePrice,
			TotalQuantity = product.TotalQuantity,
			QuantityInStore = product.QuantityInStore,
			QuantityInBackroom = product.QuantityInBackroom,
			QuantityInWarehouse = product.QuantityInWarehouse,
			VatRate = product.VatRate,
			ProviderName = product.ProviderName,
			ProviderContactDetails = product.ProviderContactDetails,

			PrintFromShop = true,
			PrintFromBackroom = true,
			PrintFromWarehouse = true,
			LabelCount = (int)Math.Round(product.TotalQuantity),
			IsSelected = isSelected
		};
	}
}
