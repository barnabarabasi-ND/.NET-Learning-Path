using Domain.Enums;

namespace Domain.Entities;

public class RiskFactorConfiguration
{
    public Guid Id { get; private set; }
    public RiskFactorLevel Level { get; private set; }
    public Guid ReferenceId { get; private set; }
    public decimal AdjustmentPercentage { get; private set; }
    public bool IsActive { get; private set; }

    private RiskFactorConfiguration() { }

    public RiskFactorConfiguration(RiskFactorLevel level, Guid referenceId,
        decimal adjustmentPercentage, bool isActive = true)
    {
        if (!Enum.IsDefined(level)) throw new ArgumentException("Risk factor level is not valid.", nameof(level));
        if (referenceId == Guid.Empty) throw new ArgumentException("Reference is required.", nameof(referenceId));

        Id = Guid.NewGuid();
        Level = level;
        ReferenceId = referenceId;
        AdjustmentPercentage = adjustmentPercentage;
        IsActive = isActive;
    }
}
