using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Your_Wallet.Models.Data;

public class MainContext : DbContext
{
    public MainContext(DbContextOptions<MainContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // from assembly Expense_Tracker.Models
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        modelBuilder.Entity<Category>().Property(c => c.Type).HasConversion<string>();
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
}