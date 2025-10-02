using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;


public interface ISubLocationService
{
    Task<Guid> CreateSubLocationAsync(SubLocationModel subLocationModel, Guid userId);
    Task UpdateSubLocationAsync(SubLocationModel subLocationModel, Guid userId);
    Task DeleteSubLocationAsync(Guid subLocationId, Guid userId);
    Task<SubLocationModel> GetSubLocationByIdAsync(Guid id, Guid userId);
    Task<PaginationResult<SubLocationModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid locationId, Page? page);
    Task<IReadOnlyList<SubLocationModel>> GetSubLocationsByUserIdAsync(Guid userId);
}