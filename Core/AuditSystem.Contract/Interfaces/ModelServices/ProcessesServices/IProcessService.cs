using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;

public interface IProcessService
{
    public Task<Guid> CreateProcessAsync(ProcessModel processModel);
}
