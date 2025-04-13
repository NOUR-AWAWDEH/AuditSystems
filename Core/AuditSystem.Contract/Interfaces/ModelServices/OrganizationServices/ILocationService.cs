using AuditSystem.Contract.Models.Organisation;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;

public interface ILocationService
{
    public Task<Guid> CreateLocationAsync(LocationModel locationModel);
    public Task UpdateLocationAsync(LocationModel locationModel);
}
