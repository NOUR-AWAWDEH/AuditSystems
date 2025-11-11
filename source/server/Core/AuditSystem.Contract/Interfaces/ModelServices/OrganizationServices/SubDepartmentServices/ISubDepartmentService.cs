using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;


public interface ISubDepartmentService
{
    Task<Guid> CreateSubDepartmentAsync(SubDepartmentModel subDepartmentModel, Guid userId);
    Task UpdateSubDepartmentAsync(SubDepartmentModel subDepartmentModel, Guid userId);
    Task DeleteSubDepartmentAsync(Guid subDepartmentId, Guid userId);
    Task<SubDepartmentModel> GetSubDepartmentByIdAsync(Guid id, Guid userId);
    Task<PaginationResult<SubDepartmentModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid departmentId, Page? page);
    Task<IReadOnlyList<SubDepartmentModel>> GetSubDepartmentsByUserIdAsync(Guid userId);
    
    // The existing SearchAsync for admin/system use if needed
    Task<PaginationResult<SubDepartmentModel>> SearchAsync(string? name, Page? page);
}