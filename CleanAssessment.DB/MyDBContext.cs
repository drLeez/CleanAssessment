using CleanAssessment.DB.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CleanAssessment.DB
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {

        }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        #region DBSets

        public string GetConnectionString => Database.GetDbConnection().ConnectionString;

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(true);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(100)
                    .IsUnicode(true);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(true);

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.AccDateId)
                    .IsRequired()
                    .HasColumnName("AccountCreationDateId")
                    .HasColumnType("int");
            });

            modelBuilder.Entity<PaymentMethod>(entity => entity.HasNoKey());
        }

        //public IEnumerable<PaymentMethod> GetPaymentMethods(int customerId)
        //    => PaymentMethods.FromSqlRaw($"EXEC GetPaymentMethods {customerId}").ToList();
    }
}
