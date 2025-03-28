using Controle_Gastos.Models;
using Microsoft.EntityFrameworkCore;

namespace Controle_Gastos.Data;

/// <summary>
/// Database context.
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use Sqlite for local development.
        optionsBuilder.UseSqlite("Data Source=controle_gastos.sqlite");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
            entity.Property(u => u.Name).IsRequired();
            entity.Property(u => u.Age).IsRequired();
            entity.Property(u => u.Role)
                .HasConversion<string>()
                .IsRequired();

            entity.HasMany(u => u.Transactions)
                .WithOne()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).ValueGeneratedOnAdd();
            entity.Property(t => t.Description).IsRequired();
            entity.Property(t => t.Amount).IsRequired();
            entity.Property(t => t.Type)
                .HasConversion<string>()
                .IsRequired();
            entity.Property(t => t.UserId).IsRequired();
        });
    }
}