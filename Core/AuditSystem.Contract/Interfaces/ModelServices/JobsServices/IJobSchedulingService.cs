using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Contract.Interfaces.ModelServices.JobsServices;

public interface IJobSchedulingService
{
    public Task<Guid> CreateJobSchedulingAsync(JobSchedulingModel jobSchedulingModel);
    public Task UpdateJobSchedulingAsync(JobSchedulingModel jobSchedulingModel);
    public Task DeleteJobSchedulingAsync(Guid jobSchedulingId);
}
