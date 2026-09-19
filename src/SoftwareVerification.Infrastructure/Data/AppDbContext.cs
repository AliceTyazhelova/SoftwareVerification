using Microsoft.EntityFrameworkCore;
using SoftwareVerification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareVerification.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        //Только чтение, чтобы нельзя было присвоить null
        public DbSet<VerificationObject> VerificationObjects => Set <VerificationObject>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Сканирует текущую сборку на наличие всех неабстрактных классов, реализующих IEntityTypeConfiguration<T>
            //Создает экземпляр каждой и вызывает ее метод Configure и применяет настройки modelBuilder
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}