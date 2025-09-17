using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;

public interface ISubLocationValidation
{
    Task<SubLocation> ValidateSubLocationOwnershipAsync(Guid subLocationId, Guid userId);
    Task<Location> ValidateLocationOwnershipAsync(Guid locationId, Guid userId);
}