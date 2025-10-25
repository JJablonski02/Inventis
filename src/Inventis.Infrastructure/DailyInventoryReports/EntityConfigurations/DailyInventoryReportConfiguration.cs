using Inventis.Domain.DailyInventoryReports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventis.Infrastructure.DailyInventoryReports.EntityConfigurations;

internal sealed class DailyInventoryReportsConfiguration : IEntityTypeConfiguration<DailyInventoryReport>
{
	public void Configure(EntityTypeBuilder<DailyInventoryReport> builder)
	{
		builder.ToTable("DailyInventoryReports");

		builder.Property(x => x.Id)
			.IsUlid()
			.IsRequired();

		builder.Property(r => r.IsClosed)
			.IsRequired();

		builder.Property(r => r.CreatedAt)
			.HasColumnType("timestamp without time zone")
			.IsRequired();

		builder.Property(r => r.ClosedAt)
			.HasColumnType("timestamp without time zone")
			.IsRequired(false);

		builder.OwnsMany(r => r.DailyScans, scan =>
		{
			scan.ToTable("DailyInventoryScans");

			scan.HasKey(y => y.Id);

			scan.Property(x => x.Id)
				.IsUlid()
				.IsRequired();

			scan.WithOwner()
				.HasForeignKey("DailyInventoryReportId");

			scan.Property(s => s.ProductId)
				.IsUlid()
				.IsRequired();

			scan.Property(s => s.IsDeleted)
				.IsRequired();

			scan.Property(s => s.Note)
				.HasMaxLength(250)
				.IsRequired(false);

			scan.Property(s => s.ScanTime)
				.HasColumnType("timestamp without time zone")
				.IsRequired();

			scan.Property(x => x.Version)
				.IsRowVersion();
		});
	}
}
