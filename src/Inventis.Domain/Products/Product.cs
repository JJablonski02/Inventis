using Inventis.Domain.Products.Constants;

namespace Inventis.Domain.Products;

public sealed class Product : Entity
{
	private readonly List<ProductInventoryMovementLog> _inventoryMovementLogs = [];

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
		decimal storedQuantityInStore,
		decimal storedQuantityInBackroom,
		decimal storedQuantityInWarehouse,
		decimal purchasePriceVatRate,
		decimal salePriceVatRate,
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

		StoredQuantityInBackroom = storedQuantityInBackroom;
		StoredQuantityInWarehouse = storedQuantityInWarehouse;
		StoredQuantityInStore = storedQuantityInStore;
		CurrentQuantityInBackroom = storedQuantityInBackroom;
		CurrentQuantityInWarehouse = storedQuantityInWarehouse;
		CurrentQuantityInStore = storedQuantityInStore;

		PurchasePriceVatRate = purchasePriceVatRate;
		SalePriceVatRate = salePriceVatRate;

		ProviderName = providerName;
		ProviderContactDetails = providerContactDetails;
		CreatedAt = DateTime.Now;

		AddInventoryMovementLog(
			InventoryMovementLogAction.Created);
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

	public decimal StoredQuantityInBackroom { get; private set; }
	public decimal StoredQuantityInWarehouse { get; private set; }
	public decimal StoredQuantityInStore { get; private set; }
	public decimal CurrentQuantityInBackroom { get; private set; }
	public decimal CurrentQuantityInWarehouse { get; private set; }
	public decimal CurrentQuantityInStore { get; private set; }
	public decimal CurrentTotalQuantity => StoredQuantityInStore + StoredQuantityInBackroom + StoredQuantityInWarehouse;
	public decimal StoredTotalQuantity => CurrentQuantityInStore + CurrentQuantityInBackroom + CurrentQuantityInWarehouse;

	public decimal PurchasePriceVatRate { get; private set; }
	public decimal SalePriceVatRate { get; private set; }

	public string? ProviderName { get; private set; }
	public string? ProviderContactDetails { get; private set; }
	public DateTime CreatedAt { get; }

	public IReadOnlyCollection<ProductInventoryMovementLog> InventoryMovementLogs => _inventoryMovementLogs.AsReadOnly();

	public static Product Create(
		string name,
		string? description,
		string eanCode,
		decimal netPurchasePrice,
		decimal grossPurchasePrice,
		decimal netSalePrice,
		decimal grossSalePrice,
		decimal storedQuantityInStore,
		decimal storedQuantityInBackroom,
		decimal storedQuantityInWarehouse,
		decimal purchasePriceVatRate,
		decimal salePriceVatRate,
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

		var totalPurchaseGrossValue = grossPurchasePrice * (storedQuantityInStore + storedQuantityInBackroom + storedQuantityInWarehouse);
		var totalSaleGrossValue = grossSalePrice * (storedQuantityInStore + storedQuantityInBackroom + storedQuantityInWarehouse);

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
			storedQuantityInStore,
			storedQuantityInBackroom,
			storedQuantityInWarehouse,
			purchasePriceVatRate,
			salePriceVatRate,
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
		decimal purchasePriceVatRate,
		decimal salePriceVatRate,
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
		CurrentQuantityInStore = quantityInStore;
		CurrentQuantityInBackroom = quantityInBackroom;
		CurrentQuantityInWarehouse = quantityInWarehouse;
		PurchasePriceVatRate = purchasePriceVatRate;
		SalePriceVatRate = salePriceVatRate;
		ProviderName = providerName;
		ProviderContactDetails = providerContactDetails;

		TotalPurchaseGrossValue = grossPurchasePrice * (quantityInStore + quantityInBackroom + quantityInWarehouse);
		TotalSaleGrossValue = grossSalePrice * (quantityInStore + quantityInBackroom + quantityInWarehouse);

		AddInventoryMovementLog(
			InventoryMovementLogAction.Updated);
	}

	public void DecreaseSingleQuantityAutomatically(Ulid scanId)
	{
		QuantityType quantityType;
		if (CurrentQuantityInStore >= 1)
		{
			CurrentQuantityInStore = DecreaseQuantity(CurrentQuantityInStore);
			quantityType = QuantityType.InStore;
		}
		else if (CurrentQuantityInBackroom >= 1)
		{
			CurrentQuantityInBackroom = DecreaseQuantity(CurrentQuantityInBackroom);
			quantityType = QuantityType.InBackroom;
		}
		else
		{
			CurrentQuantityInWarehouse = CurrentQuantityInWarehouse >= 1
				? DecreaseQuantity(CurrentQuantityInWarehouse)
				: throw new InvalidOperationException("Brak towaru w magazynie. Nie można zmniejszyć ilości, ponieważ produkt nie jest dostępny w żadnym magazynie.");

			quantityType = QuantityType.InWarehouse;
		}

		AddInventoryMovementLog(
			InventoryMovementLogAction.Scanned,
			scanId,
			quantityType);
	}

	public void IncreaseSingleQuantity(Ulid scanId)
	{
		var inventoryMovementLog = _inventoryMovementLogs.SingleOrDefault(log => log.ScanId == scanId)
			?? throw new InvalidOperationException("Invalid scan identifier");

		switch (inventoryMovementLog.QuantityType)
		{
			case var v when v == QuantityType.InStore:
				CurrentQuantityInStore += 1;
				break;

			case var v when v == QuantityType.InBackroom:
				CurrentQuantityInBackroom += 1;
				break;

			case var v when v == QuantityType.InWarehouse:
				CurrentQuantityInWarehouse += 1;
				break;

			default:
				throw new InvalidOperationException("Typ magazynu nie został przypisany do danego wpisu.");
		}

		AddInventoryMovementLog(
			InventoryMovementLogAction.ScanDeleted);
	}

	public void ReconcileInventory(
		DateTime beginFrom)
	{
		StoredQuantityInStore = CurrentQuantityInStore;
		StoredQuantityInBackroom = CurrentQuantityInBackroom;
		StoredQuantityInWarehouse = CurrentQuantityInWarehouse;

		AddInventoryMovementLog(
			InventoryMovementLogAction.ClosedReport);
	}

	private static decimal DecreaseQuantity(decimal currentQuantity)
		=> currentQuantity <= 0
			? throw new InvalidOperationException("Nie można zmniejszyć ilości poniżej zera.")
			: currentQuantity - 1;

	public void AddInventoryMovementLog(InventoryMovementLogAction action, Ulid? scanId = null, QuantityType? quantityType = null)
	{
		var lastLog = _inventoryMovementLogs
			.OrderByDescending(l => l.CreatedAt)
			.FirstOrDefault();

		var currentQuantityInStoreBefore = lastLog?.CurrentQuantityInStoreAfter
			?? this.CurrentQuantityInStore;
		var currentQuantityInBackroomBefore = lastLog?.CurrentQuantityInBackroomAfter
			?? this.CurrentQuantityInBackroom;
		var currentQuantityInWarehouseBefore = lastLog?.CurrentQuantityInWarehouseAfter
			?? this.CurrentQuantityInWarehouse;

		var storedQuantityInStoreBefore = lastLog?.StoredQuantityInStoreAfter
			?? this.StoredQuantityInStore;
		var storedQuantityInBackroomBefore = lastLog?.StoredQuantityInBackroomAfter
			?? this.StoredQuantityInBackroom;
		var storedQuantityInWarehouseBefore = lastLog?.StoredQuantityInWarehouseAfter
			?? this.StoredQuantityInWarehouse;

		var totalCurrentQuantityBefore = lastLog?.TotalCurrentQuantityAfter;
		var totalStoredQuantityBefore = lastLog?.TotalStoredQuantityAfter;

		var totalCurrentQuantityAfter = CurrentQuantityInStore + CurrentQuantityInBackroom + CurrentQuantityInWarehouse;
		var totalStoredQuantityAfter = StoredQuantityInStore + StoredQuantityInBackroom + StoredQuantityInBackroom;

		if (totalCurrentQuantityAfter == totalCurrentQuantityBefore &&
			totalStoredQuantityAfter == totalStoredQuantityBefore)
		{
			return;
		}

		var direction = totalCurrentQuantityBefore is null || totalCurrentQuantityBefore < totalCurrentQuantityAfter
			? InventoryMovementLogDirection.In
			: InventoryMovementLogDirection.Out;

		var log = ProductInventoryMovementLog.Create(
			productId: Id,
			scanId: scanId,
			quantityType: quantityType,
			action: action,
			direction: direction,
			currentQuantityInStoreBefore: currentQuantityInStoreBefore,
			currentQuantityInBackroomBefore: currentQuantityInBackroomBefore,
			currentQuantityInWarehouseBefore: currentQuantityInWarehouseBefore,
			currentQuantityInStoreAfter: CurrentQuantityInStore,
			currentQuantityInBackroomAfter: CurrentQuantityInBackroom,
			currentQuantityInWarehouseAfter: CurrentQuantityInWarehouse,
			storedQuantityInStoreBefore: storedQuantityInStoreBefore,
			storedQuantityInBackroomBefore: storedQuantityInBackroomBefore,
			storedQuantityInWarehouseBefore: storedQuantityInWarehouseBefore,
			storedQuantityInStoreAfter: StoredQuantityInStore,
			storedQuantityInBackroomAfter: StoredQuantityInBackroom,
			storedQuantityInWarehouseAfter: StoredQuantityInWarehouse
		);

		_inventoryMovementLogs.Add(log);
	}
}
