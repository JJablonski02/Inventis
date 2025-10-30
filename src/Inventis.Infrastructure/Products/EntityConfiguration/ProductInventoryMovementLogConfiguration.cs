using Inventis.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventis.Infrastructure.Products.EntityConfiguration;

internal sealed class ProductInventoryMovementLogConfiguration : IEntityTypeConfiguration<ProductInventoryMovementLog>
{
	public void Configure(EntityTypeBuilder<ProductInventoryMovementLog> builder)
	{
		builder.ToTable("InventoryMovementLogs");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.IsUlid()
			.IsRequired();

		builder.Property(x => x.ProductId)
			.IsUlid()
			.IsRequired();

		builder.Property(x => x.ScanId)
			.IsUlid()
			.IsRequired(false);

		builder.Property(x => x.Action)
			.HasMaxLength(15)
			.IsRequired();

		builder.Property(x => x.Direction)
			.HasMaxLength(3)
			.IsRequired();

		builder.Property(x => x.QuantityType)
			.HasMaxLength(15)
			.IsRequired(false);

		builder.Property(x => x.CreatedAt)
			.HasColumnType("timestamp without time zone")
			.IsRequired();

		builder.Property(x => x.CurrentQuantityInStoreBefore)
			.HasPrecision(8,2)
			.IsRequired();

		builder.Property(x => x.CurrentQuantityInBackroomBefore)
			.HasPrecision(8, 2)
			.IsRequired();

		builder.Property(x => x.CurrentQuantityInWarehouseBefore)
			.HasPrecision(8, 2)
			.IsRequired();

		builder.Property(x => x.CurrentQuantityInStoreAfter)
			   .HasPrecision(8, 2)
			   .IsRequired();

		builder.Property(x => x.CurrentQuantityInBackroomAfter)
			   .HasPrecision(8, 2)
			   .IsRequired();

		builder.Property(x => x.CurrentQuantityInWarehouseAfter)
			   .HasPrecision(8, 2)
			   .IsRequired();

		builder.Property(x => x.StoredQuantityInStoreBefore)
			   .HasPrecision(8, 2)
			   .IsRequired();

		builder.Property(x => x.StoredQuantityInBackroomBefore)
			   .HasPrecision(8, 2)
			   .IsRequired();

		builder.Property(x => x.StoredQuantityInWarehouseBefore)
			   .HasPrecision(8, 2)
			   .IsRequired();

		builder.Property(x => x.StoredQuantityInStoreAfter)
			   .HasPrecision(8, 2)
			   .IsRequired();

		builder.Property(x => x.StoredQuantityInBackroomAfter)
			   .HasPrecision(8, 2)
			   .IsRequired();

		builder.Property(x => x.StoredQuantityInWarehouseAfter)
			   .HasPrecision(8, 2)
			   .IsRequired();

		builder.Ignore(x => x.TotalStoredQuantityAfter);
		builder.Ignore(x => x.TotalCurrentQuantityAfter);
	}
}
