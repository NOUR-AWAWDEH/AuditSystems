using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;


public interface IDepartmentValidation
{
    Task<Department> ValidateDepartmentOwnershipAsync(Guid departmentId, Guid userId);
    Task<Location> ValidateLocationOwnershipAsync(Guid locationId, Guid userId);
}