using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inventis.Infrastructure.Postgres;

internal sealed class UlidToStringConverter : ValueConverter<Ulid, string>
{
	public UlidToStringConverter()
		: base(
			id => id.ToString(),
			value => Ulid.Parse(value))
	{
	}
}
