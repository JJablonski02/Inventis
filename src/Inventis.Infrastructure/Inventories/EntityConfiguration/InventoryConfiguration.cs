using Inventis.Domain.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventis.Infrastructure.Inventories.EntityConfiguration;

internal sealed class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
	public void Configure(EntityTypeBuilder<Inventory> builder)
	{
		builder.ToTable("Inventories");

		builder.HasKey(y => y.Id);

		builder.Property(i => i.Id)
			.IsUlid()
			.IsRequired();

		builder.Property(i => i.UserId)
			.IsUlid()
			.IsRequired();

		builder.Property(i => i.UserFullName)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(i => i.IsCompleted)
			.IsRequired();

		builder.Property(i => i.StartedAt)
			.IsRequired();

		builder.Property(i => i.CompletedAt)
			.IsRequired(false);

		builder.Property(i => i.Type)
			.HasColumnName("Type")
			.HasMaxLength(20)
			.IsRequired();

		builder.OwnsMany(i => i.Items, item =>
		{
			item.ToTable("InventoryItems");

			item.HasKey(y => y.Id);

			item.Property(y => y.Id)
				.IsUlid()
				.IsRequired();

			item.WithOwner()
				.HasForeignKey("InventoryId");

			item.Property(ii => ii.ProductId)
				.IsUlid()
				.IsRequired();

			item.Property(ii => ii.ProductName)
				.HasMaxLength(100)
				.IsRequired();

			item.Property(ii => ii.Quantity)
				.HasPrecision(8, 2)
				.IsRequired();

			item.Property(ii => ii.ExpectedQuantity)
				.HasPrecision(8, 2)
				.IsRequired();

			item.Property(x => x.Version)
				.IsRowVersion();
		});
	}
}
