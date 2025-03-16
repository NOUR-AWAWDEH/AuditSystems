using AuditSystem.Contract.Models.Organisation;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;

public interface IDepartmentService
{
    public Task<Guid> CreateDepartmentAsync(DepartmentModel departmentModel);
}
