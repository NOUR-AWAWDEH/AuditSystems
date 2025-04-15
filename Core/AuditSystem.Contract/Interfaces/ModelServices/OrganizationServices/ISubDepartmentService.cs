using AuditSystem.Contract.Models.Organisation;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;

public interface ISubDepartmentService
{
    public Task<Guid> CreateSubDepartmentAsync(SubDepartmentModel subDepartmentModel);
    public Task UpdateSubDepartmentAsync(SubDepartmentModel subDepartmentModel);
    public Task DeleteSubDepartmentAsync(Guid subDepartmentId);
}