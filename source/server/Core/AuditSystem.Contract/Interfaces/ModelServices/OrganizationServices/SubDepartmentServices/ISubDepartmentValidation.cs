using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;

public interface ISubDepartmentValidation
{
    Task<SubDepartment> ValidateSubDepartmentOwnershipAsync(Guid subDepartmentId, Guid userId);
    Task<Department> ValidateDepartmentOwnershipAsync(Guid departmentId, Guid userId);
}