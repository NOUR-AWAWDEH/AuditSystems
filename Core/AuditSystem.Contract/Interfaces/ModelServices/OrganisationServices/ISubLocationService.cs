using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;

public interface ISubLocationService
{
    public Task<Guid> CreateSubLocationAsync(SubProcessModel supProcessModel);
}