using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Contract.Interfaces.ModelServices.JobsServices;

public interface IJobPrioritizationService
{
    public Task<Guid> CreateJobPrioritizationAsync(JobPrioritizationModel jobPrioritizationModel);
}
