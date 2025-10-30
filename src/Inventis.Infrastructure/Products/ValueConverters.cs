using Inventis.Domain.Products.Constants;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inventis.Infrastructure.Products;

internal sealed class InventoryMovementLogActionValueConverter() : ValueConverter<InventoryMovementLogAction, string>(
	x => x.Value.ToString(),
	y => InventoryMovementLogAction.Parse(y));

internal sealed class InventoryMovementLogDirectionValueConverter() : ValueConverter<InventoryMovementLogDirection, string>(
	x => x.Value.ToString(),
	y => InventoryMovementLogDirection.Parse(y));

internal sealed class QuantityTypeValueConverter() : ValueConverter<QuantityType, string>(
	x => x.Value.ToString(),
	y => QuantityType.Parse(y));
