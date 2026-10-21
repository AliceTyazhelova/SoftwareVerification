using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareVerification.Domain.Entities;

namespace SoftwareVerification.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users"); // Используем множественное число

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Login)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        // Настройка связи Многие-ко-Многим
        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j =>
            {
                j.ToTable("UserRoles"); // Имя промежуточной таблицы в БД
            });
    }
}