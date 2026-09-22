using InsuranceApp.Domain.Constants;
using InsuranceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceApp.Infrastructure.Persistence.Configurations;

internal sealed class PolicyConfiguration : IEntityTypeConfiguration<Policy>
{
    public void Configure(EntityTypeBuilder<Policy> builder)
    {
        builder.ToTable("Policies");

        builder.HasKey(x => x.PolicyId);

        builder.Property(x => x.PolicyNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.PolicyNumber)
            .IsUnique();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .IsRequired();

        builder.Property(x => x.BasePremium)
            .HasPrecision(18, CommonConstraints.DecimalScale)
            .IsRequired();

        builder.Property(x => x.FinalPremium)
            .HasPrecision(18, CommonConstraints.DecimalScale)
            .IsRequired();

        builder.Property(x => x.CancellationReason)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.Client)
            .WithMany(x => x.Policies)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Building)
            .WithMany(x => x.Policies)
            .HasForeignKey(x => x.BuildingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Broker)
            .WithMany(x => x.Policies)
            .HasForeignKey(x => x.BrokerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Currency)
            .WithMany(x => x.Policies)
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.ClientId);
        builder.HasIndex(x => x.BuildingId);
        builder.HasIndex(x => x.BrokerId);
        builder.HasIndex(x => x.CurrencyId);
        builder.HasIndex(x => x.Status);
    }
}