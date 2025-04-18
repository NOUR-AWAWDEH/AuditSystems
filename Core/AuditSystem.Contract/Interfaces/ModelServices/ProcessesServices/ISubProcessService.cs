using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;

public interface ISubProcessService
{
    public Task<Guid> CreateSubProcessAsync(SubProcessModel subProcessModel);
    public Task UpdateSubProcessAsync(SubProcessModel subProcessModel);
    public Task DeleteSubProcessAsync(Guid subProcessId);
    public Task<SubProcessModel> GetSubProcessByIdAsync(Guid id);
}