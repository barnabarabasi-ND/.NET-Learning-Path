using InsuranceApp.Domain.Constants;
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

        builder.Property(x => x.Name).HasMaxLength(ClientConstraints.NameMaxLength).IsRequired();

        builder.Property(x => x.IdentificationNumber).HasMaxLength(ClientConstraints.IdentificationNumberMaxLength).IsRequired();
        builder.HasIndex(x => x.IdentificationNumber).IsUnique();

        builder.Property(x => x.Email).HasMaxLength(ClientConstraints.EmailMaxLength);

        builder.Property(x => x.Phone).HasMaxLength(ClientConstraints.PhoneMaxLength);

        builder.Property(x => x.Address).HasMaxLength(ClientConstraints.AddressMaxLength);

        builder.Property(x => x.CreatedAt).IsRequired();
        
    }
}