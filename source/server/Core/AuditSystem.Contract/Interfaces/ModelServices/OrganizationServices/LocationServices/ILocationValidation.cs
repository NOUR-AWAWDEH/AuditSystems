using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;

public interface ILocationValidation
{
    Task<Location> ValidateLocationOwnershipAsync(Guid locationId, Guid userId);
}