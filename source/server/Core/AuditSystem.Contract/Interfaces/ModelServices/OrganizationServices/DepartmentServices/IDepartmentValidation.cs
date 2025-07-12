using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;


public interface IDepartmentValidation
{
    Task<Department> ValidateDepartmentOwnerShipAsync(Guid departmentId, Guid userId);
    Task<SubLocation> ValidateSubLocationOwnerShipAsync(Guid subLocationId, Guid userId);
}