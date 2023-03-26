using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using System.Linq;

namespace Otus.Teaching.PromoCodeFactory.DataAccess
{
    public class DataContext
        : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Role>().HasData(FakeDataFactory.Roles);
            modelBuilder.Entity<Preference>().HasData(FakeDataFactory.Preferences);
            modelBuilder.Entity<Customer>().HasData(FakeDataFactory.Customers);
            modelBuilder.Entity<Customer>()
                .HasMany<Preference>(x => x.Preferences)
                .WithMany(c => c.Customers)
                .UsingEntity(x => x.ToTable("CustomerPreference")
                .HasData(new
                {
                    CustomersId = FakeDataFactory.Customers.First(x => x.Email == "ivan_sergeev@mail.ru").Id,
                    PreferencesId = FakeDataFactory.Preferences.First(x => x.Name == "Семья").Id
                }));

            modelBuilder.Entity<Employee>().HasOne<Role>(x => x.Role).WithMany().HasForeignKey(k => k.RoleId);
            modelBuilder.Entity<Employee>().HasData(FakeDataFactory.Employees);

            base.OnModelCreating(modelBuilder);
        }
    }
}