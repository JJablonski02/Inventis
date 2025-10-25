using Inventis.Domain.Products.Constants;

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
		CreatedAt = DateTime.Now;
	}

	public string Name { get; private set; }
	public string? Description { get; private set; }
	public string EanCode { get; private set; }
	public decimal NetPurchasePrice { get; private set; }
	public decimal GrossPurchasePrice { get; private set; }
	public decimal NetSalePrice { get; private set; }
	public decimal GrossSalePrice { get; private set; }
	public decimal TotalPurchaseGrossValue { get; private set; }
	public decimal TotalSaleGrossValue { get; private set; }
	public decimal QuantityInBackroom { get; private set; }
	public decimal QuantityInWarehouse { get; private set; }
	public decimal QuantityInStore { get; private set; }
	public decimal TotalQuantity => QuantityInStore + QuantityInBackroom + QuantityInWarehouse;
	public decimal VatRate { get; private set; }
	public string? ProviderName { get; private set; }
	public string? ProviderContactDetails { get; private set; }
	public DateTime CreatedAt { get; }

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

	public void Update(
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

		Name = name;
		Description = description;
		EanCode = eanCode;
		NetPurchasePrice = netPurchasePrice;
		GrossPurchasePrice = grossPurchasePrice;
		NetSalePrice = netSalePrice;
		GrossSalePrice = grossSalePrice;
		QuantityInStore = quantityInStore;
		QuantityInBackroom = quantityInBackroom;
		QuantityInWarehouse = quantityInWarehouse;
		VatRate = vatRate;
		ProviderName = providerName;
		ProviderContactDetails = providerContactDetails;

		TotalPurchaseGrossValue = grossPurchasePrice * (quantityInStore + quantityInBackroom + quantityInWarehouse);
		TotalSaleGrossValue = grossSalePrice * (quantityInStore + quantityInBackroom + quantityInWarehouse);
	}

	public void DecreaseSingleQuantity(QuantityType type)
	{
		switch (type)
		{
			case QuantityType.InStore:
				QuantityInStore = DecreaseQuantity(QuantityInStore);
				break;

			case QuantityType.InBackroom:
				QuantityInBackroom = DecreaseQuantity(QuantityInBackroom);
				break;

			case QuantityType.InWarehouse:
				QuantityInWarehouse = DecreaseQuantity(QuantityInWarehouse);
				break;

			default:
				throw new InvalidOperationException("Nieznany typ ilości.");
		}
	}

	private static decimal DecreaseQuantity(decimal currentQuantity)
	{
		if (currentQuantity <= 0)
			throw new InvalidOperationException("Nie można zmniejszyć ilości poniżej zera.");

		return currentQuantity - 1;
	}
}
