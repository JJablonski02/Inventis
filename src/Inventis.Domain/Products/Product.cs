namespace Inventis.Domain.Products;

public sealed class Product : Entity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	private Product() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	private Product(
		string name,
		string? description,
		string eanCode,
		decimal netPurchasePrice,
		decimal grossPurchasePrice,
		decimal netSalePrice,
		decimal grossSalePrice,
		decimal totalPurchaseGrossValue,
		decimal totalSaleGrossValue,
		decimal quantityInStore,
		decimal quantityInBackroom,
		decimal quantityInWarehouse,
		decimal vatRate,
		string? providerName,
		string? providerContactDetails)
	{
		Name = name;
		Description = description;
		EanCode = eanCode;
		NetPurchasePrice = netPurchasePrice;
		GrossPurchasePrice = grossPurchasePrice;
		NetSalePrice = netSalePrice;
		GrossSalePrice = grossSalePrice;
		TotalPurchaseGrossValue = totalPurchaseGrossValue;
		TotalSaleGrossValue = totalSaleGrossValue;
		QuantityInBackroom = quantityInBackroom;
		QuantityInWarehouse = quantityInWarehouse;
		QuantityInStore = quantityInStore;
		VatRate = vatRate;
		ProviderName = providerName;
		ProviderContactDetails = providerContactDetails;
		CreatedAt = DateTime.UtcNow;
	}

	public string Name { get; }
	public string? Description { get; }
	public string EanCode { get; }
	public decimal NetPurchasePrice { get; }
	public decimal GrossPurchasePrice { get; }
	public decimal NetSalePrice { get; }
	public decimal GrossSalePrice { get; }
	public decimal TotalPurchaseGrossValue { get; }
	public decimal TotalSaleGrossValue { get; }
	public decimal QuantityInBackroom { get; }
	public decimal QuantityInWarehouse { get; }
	public decimal QuantityInStore { get; }
	public decimal VatRate { get; }
	public string? ProviderName { get; }
	public string? ProviderContactDetails { get; }
	public DateTime CreatedAt { get; }

	public decimal TotalQuantity => QuantityInBackroom + QuantityInWarehouse + QuantityInStore;

	public static Product Create(
		string name,
		string? description,
		string eanCode,
		decimal netPurchasePrice,
		decimal grossPurchasePrice,
		decimal netSalePrice,
		decimal grossSalePrice,
		decimal quantityInStore,
		decimal quantityInBackroom,
		decimal quantityInWarehouse,
		decimal vatRate,
		string? providerName = null,
		string? providerContactDetails = null)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Name cannot be empty", nameof(name));
		}

		if (string.IsNullOrWhiteSpace(eanCode))
		{
			throw new ArgumentException("EAN code cannot be empty", nameof(eanCode));
		}

		if (netPurchasePrice < 0 || grossPurchasePrice < 0 || netSalePrice < 0 || grossSalePrice < 0)
		{
			throw new ArgumentException("Prices cannot be negative");
		}

		var totalPurchaseGrossValue = grossPurchasePrice * (quantityInStore + quantityInBackroom + quantityInWarehouse);
		var totalSaleGrossValue = grossSalePrice * (quantityInStore + quantityInBackroom + quantityInWarehouse);

		return new Product(
			name,
			description,
			eanCode,
			netPurchasePrice,
			grossPurchasePrice,
			netSalePrice,
			grossSalePrice,
			totalPurchaseGrossValue,
			totalSaleGrossValue,
			quantityInStore,
			quantityInBackroom,
			quantityInWarehouse,
			vatRate,
			providerName,
			providerContactDetails);
	}
}
