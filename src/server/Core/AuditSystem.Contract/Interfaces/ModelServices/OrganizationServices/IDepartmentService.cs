using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

public interface IDepartmentService
{
    public Task<Guid> CreateDepartmentAsync(DepartmentModel departmentModel);
    public Task UpdateDepartmentAsync(DepartmentModel departmentModel);
    public Task DeleteDepartmentAsync(Guid departmentId);
    public Task<DepartmentModel> GetDepartmentByIdAsync(Guid id);
    public Task<PaginationResult<DepartmentModel>> SearchAsync(string? name, Page page);
}
