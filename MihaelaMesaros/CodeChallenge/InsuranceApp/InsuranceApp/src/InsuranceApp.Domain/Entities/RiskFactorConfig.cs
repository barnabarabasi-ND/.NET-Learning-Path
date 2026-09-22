using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Domain.Entities;

public sealed class RiskFactorConfig
{
    public int RiskFactorConfigurationId { get; set; }

    public RiskFactorLevel Level { get; set; }

    public int ReferenceId { get; set; }

    public decimal AdjustmentPercentage { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }
}