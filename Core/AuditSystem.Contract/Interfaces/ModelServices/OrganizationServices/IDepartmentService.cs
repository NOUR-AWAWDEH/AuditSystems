using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

public interface IDepartmentService
{
    public Task<Guid> CreateDepartmentAsync(DepartmentModel departmentModel);
    public Task UpdateDepartmentAsync(DepartmentModel departmentModel);
    public Task DeleteDepartmentAsync(Guid departmentId);
    public Task<DepartmentModel> GetDepartmentByIdAsync(Guid id);
}
