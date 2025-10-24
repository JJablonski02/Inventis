using Inventis.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventis.Infrastructure.Products.EntityConfiguration;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
	public void Configure(EntityTypeBuilder<Product> builder)
	{
		builder.ToTable("Products");

		builder.HasKey(y => y.Id);

		builder.Property(x => x.Id)
			.IsUlid()
			.IsRequired();

		builder.Property(p => p.Name)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(p => p.Description)
			.HasMaxLength(250)
			.IsRequired(false);

		builder.Property(p => p.EanCode)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(p => p.ProviderName)
			.HasMaxLength(100)
			.IsRequired(false);

		builder.Property(p => p.ProviderContactDetails)
			.IsRequired(false);

		builder.Property(p => p.NetPurchasePrice)
			.HasPrecision(16, 2)
			.IsRequired();

		builder.Property(p => p.GrossPurchasePrice)
			.HasPrecision(16, 2)
			.IsRequired();

		builder.Property(p => p.NetSalePrice)
			.HasPrecision(16, 2)
			.IsRequired();

		builder.Property(p => p.GrossSalePrice)
			.HasPrecision(16, 2)
			.IsRequired();

		builder.Property(p => p.TotalPurchaseGrossValue)
			.HasPrecision(16, 2)
			.IsRequired();

		builder.Property(p => p.TotalSaleGrossValue)
			.HasPrecision(16, 2)
			.IsRequired();

		builder.Property(p => p.QuantityInBackroom)
			.HasPrecision(8, 2)
			.IsRequired();

		builder.Property(p => p.QuantityInWarehouse)
			.HasPrecision(8, 2)
			.IsRequired();

		builder.Property(p => p.QuantityInStore)
			.HasPrecision(8, 2)
			.IsRequired();

		builder.Property(p => p.VatRate)
			.HasPrecision(5, 2)
			.IsRequired();

		builder.Property(p => p.CreatedAt)
			.IsRequired();

		builder.Property(x => x.Version)
			.IsRowVersion();
	}
}
