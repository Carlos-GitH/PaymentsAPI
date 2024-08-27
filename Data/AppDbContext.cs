

using Microsoft.EntityFrameworkCore;
using PaymentsApi.DTOs;
using PaymentsApi.Models;

namespace PaymentsApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base (options) { }
        public DbSet<ApiKeys> ApiKeys { get; set; }
        public DbSet<Tokens> Tokens { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<CreditCards> CreditCards { get; set; }
        public DbSet<DebitCards> DebitCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiKeys>().ToTable("api_keys");
            modelBuilder.Entity<Tokens>().ToTable("tokens");
            modelBuilder.Entity<Payments>().ToTable("payments");
            modelBuilder.Entity<CreditCards>().ToTable("credit_cards");
            modelBuilder.Entity<DebitCards>().ToTable("debit_cards");

            modelBuilder.Entity<CreditCardDTO>() .HasNoKey();
            modelBuilder.Entity<DebitCardDTO>()  .HasNoKey();
            modelBuilder.Entity<PaymentDTO>()    .HasNoKey();
        }
    }
}