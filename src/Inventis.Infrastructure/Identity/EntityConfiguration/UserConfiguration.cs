using Inventis.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventis.Infrastructure.Identity.EntityConfiguration;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("Users");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.IsUlid()
			.IsRequired();

		builder.Property(x => x.Username)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(x => x.FirstName)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(x => x.LastName)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(x => x.Password)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(x => x.Version)
			.IsRowVersion();
	}
}
