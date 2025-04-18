using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

public interface ILocationService
{
    public Task<Guid> CreateLocationAsync(LocationModel locationModel);
    public Task UpdateLocationAsync(LocationModel locationModel);
    public Task DeleteLocationAsync(Guid locationId);
    public Task<LocationModel> GetLocationByIdAsync(Guid id);
}
