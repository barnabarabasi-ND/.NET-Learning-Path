namespace Infrastructure.Persistence;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class InsuranceDbContext : DbContext
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Building> Buildings => Set<Building>();

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<County> Counties => Set<County>();
    public DbSet<City> Cities => Set<City>();

    public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(InsuranceDbContext).Assembly);
    }
}