using InsuranceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceApp.Infrastructure.Persistence.Configurations;

internal sealed class FeeConfigConfiguration : IEntityTypeConfiguration<FeeConfig>
{
    public void Configure(EntityTypeBuilder<FeeConfig> builder)
    {
        builder.ToTable("FeeConfigs");

        builder.HasKey(x => x.FeeConfigurationId);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.FeeType)
            .IsRequired();

        builder.Property(x => x.Percentage)
            .HasPrecision(7, 4)
            .IsRequired();

        builder.Property(x => x.EffectiveFrom)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}