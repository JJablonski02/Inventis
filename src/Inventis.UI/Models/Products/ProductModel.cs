using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using Inventis.Application.Products.Dtos;

namespace Inventis.UI.Models.Products;

internal sealed partial class ProductModel : ObservableObject
{
	public ProductModel()
	{
		_purchasePriceVatRate = VatRates[0];
		_salePriceVatRate = VatRates[3];
	}

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
	private VatRateModel _purchasePriceVatRate;

	[ObservableProperty]
	private VatRateModel _salePriceVatRate;

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

	public ObservableCollection<VatRateModel> VatRates { get; } =
		[
			new("0%", 0m),
			new("5%", 5m),
			new("8%", 8m),
			new("23%", 23m)
		];

	partial void OnNetPurchasePriceChanged(string? value)
	{
		var cleaned = value?.Replace("zł", "").Trim();
		NetPurchasePriceReal = TryParseDecimal(cleaned);

		if (NetPurchasePriceReal.HasValue && PurchasePriceVatRate?.Value != null)
		{
			var vatRate = PurchasePriceVatRate.Value / 100m;
			var gross = NetPurchasePriceReal.Value * (1 + vatRate);
			GrossPurchasePriceReal = Math.Round(gross, 2);
			GrossPurchasePrice = $"{GrossPurchasePriceReal:0.00}zł";
		}

		UpdateTotals();
	}

	partial void OnGrossPurchasePriceChanged(string? value)
	{
		var cleaned = value?.Replace("zł", "").Trim();
		GrossPurchasePriceReal = TryParseDecimal(cleaned);

		if (GrossPurchasePriceReal.HasValue && PurchasePriceVatRate?.Value != null)
		{
			var vatRate = PurchasePriceVatRate.Value / 100m;
			var net = GrossPurchasePriceReal.Value / (1 + vatRate);
			NetPurchasePriceReal = Math.Round(net, 2);
			NetPurchasePrice = $"{NetPurchasePriceReal:0.00}zł";
		}

		UpdateTotals();
	}

	partial void OnNetSalePriceChanged(string? value)
	{
		var cleaned = value?.Replace("zł", "").Trim();
		NetSalePriceReal = TryParseDecimal(cleaned);

		if (NetSalePriceReal.HasValue && SalePriceVatRate?.Value != null)
		{
			var vatRate = SalePriceVatRate.Value / 100m;
			var gross = NetSalePriceReal.Value * (1 + vatRate);
			GrossSalePriceReal = Math.Round(gross, 2);
			GrossSalePrice = $"{GrossSalePriceReal:0.00}zł";
		}

		UpdateTotals();
	}

	partial void OnGrossSalePriceChanged(string? value)
	{
		var cleaned = value?.Replace("zł", "").Trim();
		GrossSalePriceReal = TryParseDecimal(cleaned);

		if (GrossSalePriceReal.HasValue && SalePriceVatRate?.Value != null)
		{
			var vatRate = SalePriceVatRate.Value / 100m;
			var net = GrossSalePriceReal.Value / (1 + vatRate);
			NetSalePriceReal = Math.Round(net, 2);
			NetSalePrice = $"{NetSalePriceReal:0.00}zł";
		}

		UpdateTotals();
	}

	partial void OnPurchasePriceVatRateChanged(VatRateModel value)
	{
		if (NetPurchasePriceReal.HasValue && value?.Value != null)
		{
			var vatRate = value.Value / 100m;
			var gross = NetPurchasePriceReal.Value * (1 + vatRate);
			GrossPurchasePriceReal = Math.Round(gross, 2);
			GrossPurchasePrice = $"{GrossPurchasePriceReal:0.00}zł";
		}

		UpdateTotals();
	}

	partial void OnSalePriceVatRateChanged(VatRateModel value)
	{
		if (GrossSalePriceReal.HasValue && value?.Value != null)
		{
			var vatRate = value.Value / 100m;
			var net = GrossSalePriceReal.Value / (1 + vatRate);
			NetSalePriceReal = Math.Round(net, 2);
			NetSalePrice = $"{NetSalePriceReal:0.00}zł";
		}

		UpdateTotals();
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
		UpdateTotals();
	}

	partial void OnQuantityInBackroomChanged(string? value)
	{
		var cleaned = value?.Replace("szt", "").Trim();
		QuantityInBackroomReal = TryParseDecimal(cleaned);
		UpdateTotals();
	}

	partial void OnQuantityInWarehouseChanged(string? value)
	{
		var cleaned = value?.Replace("szt", "").Trim();
		QuantityInWarehouseReal = TryParseDecimal(cleaned);
		UpdateTotals();
	}

	private static decimal? TryParseDecimal(string? value)
	{
		if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out var result))
		{
			return result;
		}

		return null;
	}

	private void UpdateTotals()
	{
		var totalQty = (QuantityInStoreReal ?? 0)
					 + (QuantityInBackroomReal ?? 0)
					 + (QuantityInWarehouseReal ?? 0);

		if (GrossPurchasePriceReal.HasValue)
		{
			TotalPurchaseGrossValueReal = Math.Round(totalQty * GrossPurchasePriceReal.Value, 2);
			TotalPurchaseGrossValue = $"{TotalPurchaseGrossValueReal:0.00} zł";
		}
		else
		{
			TotalPurchaseGrossValueReal = null;
			TotalPurchaseGrossValue = null;
		}

		if (GrossSalePriceReal.HasValue)
		{
			TotalSaleGrossValueReal = Math.Round(totalQty * GrossSalePriceReal.Value, 2);
			TotalSaleGrossValue = $"{TotalSaleGrossValueReal:0.00} zł";
		}
		else
		{
			TotalSaleGrossValueReal = null;
			TotalSaleGrossValue = null;
		}
	}

	public void FromDto(ProductDto dto)
	{
		Id = dto.Id;
		Name = dto.Name;
		Description = dto.Description;
		EanCode = dto.EanCode;

		PurchasePriceVatRate = VatRates.Single(vatRate => vatRate.Value == dto.PurchasePriceVatRate);
		SalePriceVatRate = VatRates.Single(vatRate => vatRate.Value == dto.SalePriceVatRate);

		NetPurchasePrice = $"{dto.NetPurchasePrice.ToString("N2", CultureInfo.CurrentCulture)}zł";
		GrossPurchasePrice = $"{dto.GrossPurchasePrice.ToString("N2", CultureInfo.CurrentCulture)}zł";
		NetSalePrice = $"{dto.NetSalePrice.ToString("N2", CultureInfo.CurrentCulture)}zł";
		GrossSalePrice = $"{dto.GrossSalePrice.ToString("N2", CultureInfo.CurrentCulture)}zł";

		TotalPurchaseGrossValue = $"{(dto.GrossPurchasePrice * dto.CurrentTotalQuantity).ToString("N2", CultureInfo.CurrentCulture)}zł";
		TotalSaleGrossValue = $"{(dto.GrossSalePrice * dto.CurrentTotalQuantity).ToString("N2", CultureInfo.CurrentCulture)}zł";

		QuantityInStore = $"{dto.CurrentQuantityInStore}szt";
		QuantityInBackroom = $"{dto.CurrentQuantityInBackroom}szt";
		QuantityInWarehouse = $"{dto.CurrentQuantityInWarehouse}szt";

		ProviderName = dto.ProviderName;
		ProviderContactDetails = dto.ProviderContactDetails;

		NetPurchasePriceReal = dto.NetPurchasePrice;
		GrossPurchasePriceReal = dto.GrossPurchasePrice;
		NetSalePriceReal = dto.NetSalePrice;
		GrossSalePriceReal = dto.GrossSalePrice;
		QuantityInStoreReal = dto.CurrentQuantityInStore;
		QuantityInBackroomReal = dto.CurrentQuantityInBackroom;
		QuantityInWarehouseReal = dto.CurrentQuantityInWarehouse;
		TotalPurchaseGrossValueReal = dto.GrossPurchasePrice * dto.CurrentTotalQuantity;
		TotalSaleGrossValueReal = dto.GrossSalePrice * dto.CurrentTotalQuantity;
	}
}
