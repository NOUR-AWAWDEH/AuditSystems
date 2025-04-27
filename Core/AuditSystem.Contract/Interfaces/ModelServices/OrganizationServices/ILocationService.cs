using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

public interface ILocationService
{
    public Task<Guid> CreateLocationAsync(LocationModel locationModel);
    public Task UpdateLocationAsync(LocationModel locationModel);
    public Task DeleteLocationAsync(Guid locationId);
    public Task<LocationModel> GetLocationByIdAsync(Guid id);
    public Task<PaginationResult<LocationModel>> SearchAsync(string? name, Page page);
}
