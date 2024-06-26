using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Your_Wallet.Areas.Identity.Data;

namespace Your_Wallet.Models.Data;

public class MainContext : IdentityDbContext<ApplicationUser>
{
    public MainContext(DbContextOptions<MainContext> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // from assembly Expense_Tracker.Models
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Category>().Property(c => c.Type).HasConversion<string>();

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Categories)
            .WithOne(c => c.ApplicationUser)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete for categories


        modelBuilder.Entity<Category>()
            .HasMany(c => c.Transactions)
            .WithOne(t => t.Category)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete for transactions related to categories

    }
}