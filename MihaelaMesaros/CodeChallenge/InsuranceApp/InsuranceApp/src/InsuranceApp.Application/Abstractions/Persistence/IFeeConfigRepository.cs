using InsuranceApp.Domain.Entities;

namespace InsuranceApp.Application.Abstractions.Persistence;

public interface IFeeConfigRepository
{
    Task<IReadOnlyList<FeeConfig>> GetFeeConfigsAsync(CancellationToken cancellationToken);

    Task<FeeConfig?> GetFeeConfigByIdAsync(int feeConfigId, CancellationToken cancellationToken);

    Task<FeeConfig?> GetFeeConfigForUpdateAsync(int feeConfigId, CancellationToken cancellationToken);

    Task AddFeeConfigAsync(FeeConfig feeConfig, CancellationToken cancellationToken);

    Task SaveFeeConfigChangesAsync(CancellationToken cancellationToken);
}
