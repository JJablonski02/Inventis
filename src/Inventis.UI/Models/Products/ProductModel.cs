using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventis.UI.Models.Products;

public partial class ProductModel : ObservableObject
{
	[ObservableProperty]
	private Ulid? _id;

	[ObservableProperty]
	private string? _name;

	[ObservableProperty]
	private string? _description;

	[ObservableProperty]
	private string? _eanCode;

	[ObservableProperty]
	private string? _netPurchasePrice;

	[ObservableProperty]
	private string? _grossPurchasePrice;

	[ObservableProperty]
	private string? _netSalePrice;

	[ObservableProperty]
	private string? _grossSalePrice;

	[ObservableProperty]
	private string? _totalPurchaseGrossValue;

	[ObservableProperty]
	private string? _totalSaleGrossValue;

	[ObservableProperty]
	private string? _quantityInStore;

	[ObservableProperty]
	private string? _quantityInBackroom;

	[ObservableProperty]
	private string? _quantityInWarehouse;

	[ObservableProperty]
	private string? _vatRate = "23%";

	[ObservableProperty]
	private string? _providerName;

	[ObservableProperty]
	private string? _providerContactDetails;

	public decimal? NetPurchasePriceReal { get; private set; }
	public decimal? GrossPurchasePriceReal { get; private set; }
	public decimal? NetSalePriceReal { get; private set; }
	public decimal? GrossSalePriceReal { get; private set; }
	public decimal? TotalPurchaseGrossValueReal { get; private set; }
	public decimal? TotalSaleGrossValueReal { get; private set; }
	public decimal? QuantityInStoreReal { get; private set; }
	public decimal? QuantityInBackroomReal { get; private set; }
	public decimal? QuantityInWarehouseReal { get; private set; }
	public decimal? VatRateReal { get; private set; }

	partial void OnNetPurchasePriceChanged(string? value)
	{
		var cleaned = value?.Replace("zł", "").Trim();
		NetPurchasePriceReal = TryParseDecimal(cleaned);
	}

	partial void OnGrossPurchasePriceChanged(string? value)
	{
		var cleaned = value?.Replace("zł", "").Trim();
		GrossPurchasePriceReal = TryParseDecimal(cleaned);
	}

	partial void OnNetSalePriceChanged(string? value)
	{
		var cleaned = value?.Replace("zł", "").Trim();
		NetSalePriceReal = TryParseDecimal(cleaned);
	}

	partial void OnGrossSalePriceChanged(string? value)
	{
		var cleaned = value?.Replace("zł", "").Trim();
		GrossSalePriceReal = TryParseDecimal(cleaned);
	}

	partial void OnTotalPurchaseGrossValueChanged(string? value)
	{
		var cleaned = value?.Replace("zł", "").Trim();
		TotalPurchaseGrossValueReal = TryParseDecimal(cleaned);
	}

	partial void OnTotalSaleGrossValueChanged(string? value)
	{
		var cleaned = value?.Replace("zł", "").Trim();
		TotalSaleGrossValueReal = TryParseDecimal(cleaned);
	}

	partial void OnQuantityInStoreChanged(string? value)
	{
		var cleaned = value?.Replace("szt", "").Trim();
		QuantityInStoreReal = TryParseDecimal(cleaned);
	}

	partial void OnQuantityInBackroomChanged(string? value)
	{
		var cleaned = value?.Replace("szt", "").Trim();
		QuantityInBackroomReal = TryParseDecimal(cleaned);
	}

	partial void OnQuantityInWarehouseChanged(string? value)
	{
		var cleaned = value?.Replace("szt", "").Trim();
		QuantityInWarehouseReal = TryParseDecimal(cleaned);
	}

	partial void OnVatRateChanged(string? value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			var cleaned = value.Replace("%", "").Trim();
			VatRateReal = TryParseDecimal(cleaned);
		}
		else
		{
			VatRateReal = null;
		}
	}

	private static decimal? TryParseDecimal(string? value)
	{
		if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out var result))
		{
			return result;
		}

		return null;
	}
}
