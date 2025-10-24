using Inventis.Domain.Inventories.Constants;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inventis.Infrastructure.Inventories;

internal sealed class InventoryTypeValueConverter() : ValueConverter<InventoryType, string>(
	x => x.Value.ToString(),
	y => InventoryType.Parse(y));
