namespace Inventis.Infrastructure.Configuration;

/// <summary>
/// Inventis app settings json options
/// </summary>
public sealed class InventisOptions
{
	public required string ConnectionString { get; init; }
}
