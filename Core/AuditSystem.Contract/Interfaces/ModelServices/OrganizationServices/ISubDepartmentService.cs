using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

public interface ISubDepartmentService
{
    public Task<Guid> CreateSubDepartmentAsync(SubDepartmentModel subDepartmentModel);
    public Task UpdateSubDepartmentAsync(SubDepartmentModel subDepartmentModel);
    public Task DeleteSubDepartmentAsync(Guid subDepartmentId);
    public Task<SubDepartmentModel> GetSubDepartmentByIdAsync(Guid id);
    public Task<PaginationResult<SubDepartmentModel>> SearchAsync(string? name, Page page);
}