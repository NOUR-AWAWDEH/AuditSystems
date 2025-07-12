using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;


public interface ISubLocationService
{
    Task<Guid> CreateSubLocationAsync(SubLocationModel subLocationModel, Guid userId);
    Task UpdateSubLocationAsync(SubLocationModel subLocationModel, Guid userId);
    Task DeleteSubLocationAsync(Guid subLocationId, Guid userId);
    Task<SubLocationModel> GetSubLocationByIdAsync(Guid id, Guid userId);
    Task<PaginationResult<SubLocationModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid? locationId, Page? page);
    Task<ICollection<SubLocationModel>> GetSubLocationsByUserIdAsync(Guid userId);
    
    // The existing SearchAsync for admin/system use if needed
    Task<PaginationResult<SubLocationModel>> SearchAsync(string? name, Page? page);
}