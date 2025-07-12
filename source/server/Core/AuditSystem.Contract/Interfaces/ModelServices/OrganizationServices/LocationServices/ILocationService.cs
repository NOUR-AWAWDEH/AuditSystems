using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;

public interface ILocationService
{
    Task<Guid> CreateLocationAsync(LocationModel locationModel, Guid userId);
    Task UpdateLocationAsync(LocationModel locationModel, Guid userId);
    Task DeleteLocationAsync(Guid locationId, Guid userId);
    Task<LocationModel> GetLocationByIdAsync(Guid id, Guid userId);
    Task<ICollection<LocationModel>> GetLocationsByUserIdAsync(Guid userId);
    Task<PaginationResult<LocationModel>> SearchByUserIdAsync(Guid userId, string? name, Page page);
}
