using Microsoft.EntityFrameworkCore;
using OrderProcessingDiDemo.Domain.Orders;

namespace OrderProcessingDiDemo.Infrastructure.Persistence;

public class OrderDbContext(
    DbContextOptions<OrderDbContext> options
) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(order => order.CustomerEmail)
                .HasMaxLength(320)
                .IsRequired();

            entity.Property(order => order.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            entity.HasMany(order => order.Lines)
                .WithOne()
                .HasForeignKey(line => line.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.Property(line => line.ProductId)
                .IsRequired();

            entity.Property(line => line.Quantity)
                .IsRequired();
        });
    }
}
