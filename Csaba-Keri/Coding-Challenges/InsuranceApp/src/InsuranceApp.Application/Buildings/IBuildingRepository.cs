using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Domain.Buildings;

namespace InsuranceApp.Application.Buildings;

public interface IBuildingRepository
{
    Task<Building?> GetByIdAsync(Guid buildingId, CancellationToken cancellationToken);

    Task<PagedResult<Building>> GetByClientIdAsync(Guid clientId, PageQuery query, CancellationToken cancellationToken);

    Task AddAsync(Building building, CancellationToken cancellationToken);

    Task UpdateAsync(Building building, CancellationToken cancellationToken);
}
