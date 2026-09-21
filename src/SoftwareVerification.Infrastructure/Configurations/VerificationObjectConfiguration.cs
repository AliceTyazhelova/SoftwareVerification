using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareVerification.Domain.Entities;
using SoftwareVerification.Domain.Enums;

namespace SoftwareVerification.Infrastructure.Configurations
{
    public class VerificationObjectConfiguration : IEntityTypeConfiguration<VerificationObject>
    {
        public void Configure(EntityTypeBuilder<VerificationObject> builder)
        {
            builder.ToTable("verification_objects");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Version)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(30)
                .HasConversion<string>(); //enum по умолчанию EF хранит как int

            builder.Property(x => x.Result)
                .IsRequired()
                .HasMaxLength(30)
                .HasConversion<string>()
                .HasDefaultValue(VerificationResult.InProgress); 

            builder.Property(x => x.ExpertComment)
                .HasMaxLength(2000);
        }
    }
}
