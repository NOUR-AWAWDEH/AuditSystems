using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;

public interface ISubLocationValidation
{
    Task<Location> ValidateLocationOwnershipAsync(Guid locationId, Guid userId);
    Task<SubLocation> ValidateSubLocationOwnershipAsync(SubLocation subLocation, Guid userId);
}