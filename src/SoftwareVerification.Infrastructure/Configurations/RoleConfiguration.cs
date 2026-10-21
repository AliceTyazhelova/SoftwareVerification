using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareVerification.Domain.Entities;
using SoftwareVerification.Domain.Enums;
using System.Reflection.Emit;

namespace SoftwareVerification.Infrastructure.Configurations
{
    internal class RoleConfiguration: IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.HasIndex(r => r.Name)
            .IsUnique();

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

        }
    }
}
