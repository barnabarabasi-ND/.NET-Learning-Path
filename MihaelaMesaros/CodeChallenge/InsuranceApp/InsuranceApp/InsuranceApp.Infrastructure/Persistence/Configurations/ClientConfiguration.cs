using InsuranceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceApp.Infrastructure.Persistence.Configurations;

internal sealed class ClientConfiguration
    : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(x => x.ClientId);

        builder.Property(x => x.ClientId).ValueGeneratedOnAdd();

        builder.Property(x => x.ClientType).IsRequired();

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

        builder.Property(x => x.IdentificationNumber).HasMaxLength(50).IsRequired();

        builder.Property(x => x.Email).HasMaxLength(200);

        builder.Property(x => x.Phone).HasMaxLength(50);

        builder.Property(x => x.Address).HasMaxLength(300);

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => x.IdentificationNumber).IsUnique();
    }
}