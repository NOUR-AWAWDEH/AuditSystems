using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

public interface ISubLocationService
{
    public Task<Guid> CreateSubLocationAsync(SubLocationModel subLocationModel);
    public Task UpdateSubLocationAsync(SubLocationModel subLocationModel);
    public Task DeleteSubLocationAsync(Guid subLocationId);
    public Task<SubLocationModel> GetSubLocationByIdAsync(Guid Id);
}