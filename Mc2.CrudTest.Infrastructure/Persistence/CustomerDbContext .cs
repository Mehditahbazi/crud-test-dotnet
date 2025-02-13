using Mc2.CrudTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Infrastructure.Persistence
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasKey(c => c.Id);
            modelBuilder.Entity<Customer>().Property(c => c.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Customer>().Property(c => c.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Customer>().Property(c => c.Email).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();
            modelBuilder.Entity<Customer>().Property(c => c.PhoneNumber).HasMaxLength(20);
            modelBuilder.Entity<Customer>().Property(c => c.BankAccountNumber).HasMaxLength(50);
            modelBuilder.Entity<Customer>().Property(c => c.TimeStamp).IsRowVersion().IsConcurrencyToken();

            base.OnModelCreating(modelBuilder);
        }
    }
}
