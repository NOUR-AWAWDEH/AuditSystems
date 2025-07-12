using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;


public interface IDepartmentService
{
    Task<Guid> CreateDepartmentAsync(DepartmentModel departmentModel, Guid userId );
    Task UpdateDepartmentAsync(DepartmentModel departmentModel, Guid userId);
    Task DeleteDepartmentAsync(Guid departmentId, Guid userId); 
    Task<DepartmentModel> GetDepartmentByIdAsync(Guid id, Guid userId);
    Task<PaginationResult<DepartmentModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid? subLocationId, Page? page);
    Task<ICollection<DepartmentModel>> GetDepartmentsByUserIdAsync(Guid userId);
    
    // The existing SearchAsync for admin/system use if needed
    Task<PaginationResult<DepartmentModel>> SearchAsync(string? name, Page page);
}
