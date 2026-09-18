using InsuranceApp.Application.Buildings.Commands;
using InsuranceApp.Application.Buildings.Results;
using InsuranceApp.Application.Common.Pagination;

namespace InsuranceApp.Application.Buildings;

public interface IBuildingService
{
    Task<BuildingDetailsResult> GetByIdAsync(Guid buildingId, CancellationToken cancellationToken);

    Task<PagedResult<BuildingResult>> GetByClientIdAsync(Guid clientId, PageQuery query, CancellationToken cancellationToken);

    Task<BuildingDetailsResult> CreateAsync(CreateBuildingCommand command, CancellationToken cancellationToken);

    Task<BuildingDetailsResult> UpdateAsync(UpdateBuildingCommand command, CancellationToken cancellationToken);
}
