namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

internal static class PropertyBuilderExtensions
{
	public static PropertyBuilder<T> IsUlid<T>(this PropertyBuilder<T> builder)
	{
		return builder.HasMaxLength(26);
	}
}

