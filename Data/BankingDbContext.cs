using Microsoft.EntityFrameworkCore;
using BankingManagementSystemService.Models;

namespace BankingManagement.API.Data
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>().HasKey(a => a.AccountId);
            modelBuilder.Entity<Transactions>().HasKey(a => a.TransactionId);
            modelBuilder.Entity<Users>().HasKey(a => a.UserId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
    }
}