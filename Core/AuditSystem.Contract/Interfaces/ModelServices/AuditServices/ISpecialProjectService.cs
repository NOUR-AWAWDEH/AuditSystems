using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface ISpecialProjectService
{
    public Task<Guid> CreateSpecialProjectAsync(SpecialProjectModel specialProjectModel);
    public Task UpdateSpecialProjectAsync(SpecialProjectModel specialProjectModel);
    public Task DeleteSpecialProjectAsync(Guid specialProjectId);
    public Task<SpecialProjectModel> GetSpecialProjectByIdAsync(Guid id);
}
