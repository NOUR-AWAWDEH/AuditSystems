using AuditSystem.Contract.Models.Organisation;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;

public interface ISubLocationService
{
    public Task<Guid> CreateSubLocationAsync(SubLocationModel subLocationModel);
}