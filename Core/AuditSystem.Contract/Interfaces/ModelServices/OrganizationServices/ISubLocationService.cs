using AuditSystem.Contract.Models.Organisation;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;

public interface ISubLocationService
{
    public Task<Guid> CreateSubLocationAsync(SubLocationModel subLocationModel);
    public Task UpdateSubLocationAsync(SubLocationModel subLocationModel);
    public Task DeleteSubLocationAsync(Guid subLocationId);
}