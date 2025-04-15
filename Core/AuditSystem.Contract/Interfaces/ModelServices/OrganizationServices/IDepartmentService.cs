using AuditSystem.Contract.Models.Organisation;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;

public interface IDepartmentService
{
    public Task<Guid> CreateDepartmentAsync(DepartmentModel departmentModel);
    public Task UpdateDepartmentAsync(DepartmentModel departmentModel);
    public Task DeleteDepartmentAsync(Guid departmentId);
}
