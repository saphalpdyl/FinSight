using Fin.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fin.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<FinsightUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<Account> Accounts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Transaction>().ToTable("transactions");
            modelBuilder.Entity<Account>().ToTable("accounts");
        }
    }
}