using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
namespace Otus.Teaching.PromoCodeFactory.DataAccess
{
    public class DataContext
        : DbContext
    {
        public DbSet<PromoCode> PromoCodes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Preference> Preferences { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(FakeDataFactory.Roles);
            modelBuilder.Entity<Customer>().HasData(FakeDataFactory.Customers);
            modelBuilder.Entity<Preference>().HasData(FakeDataFactory.Preferences);
            modelBuilder.Entity<Employee>().HasOne<Role>(x => x.Role).WithMany().HasForeignKey(k=>k.RoleId);
            modelBuilder.Entity<Employee>().HasData(FakeDataFactory.Employees);
        }
    }
}
