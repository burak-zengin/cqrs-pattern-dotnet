using Domain.Products;
using Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Write.Api.Infrastructure.Persistence;

public class WriteDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public WriteDbContext(IConfiguration configuration)
    {
        _configuration = configuration;

        Database.EnsureCreated();
    }

    public DbSet<Product> Products { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSql"));

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Color).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Size).IsRequired().HasMaxLength(50);

            entity.Property(p => p.Barcode)
                .IsRequired()
                .HasMaxLength(14)
                .HasConversion(
                    barcode => barcode.Value,
                    value => Barcode.FromPersistence(value));

            entity.Ignore(p => p.DomainEvents);
        });

        base.OnModelCreating(modelBuilder);
    }
}
