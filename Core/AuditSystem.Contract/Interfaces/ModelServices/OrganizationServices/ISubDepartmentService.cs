using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

public interface ISubDepartmentService
{
    public Task<Guid> CreateSubDepartmentAsync(SubDepartmentModel subDepartmentModel);
    public Task UpdateSubDepartmentAsync(SubDepartmentModel subDepartmentModel);
    public Task DeleteSubDepartmentAsync(Guid subDepartmentId);
    public Task<SubDepartmentModel> GetSubDepartmentByIdAsync(Guid Id);
}