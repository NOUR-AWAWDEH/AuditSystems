﻿using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Contract.Interfaces.ModelServices.JobsServices;

public interface IJobPrioritizationService
{
    public Task<Guid> CreateJobPrioritizationAsync(JobPrioritizationModel jobPrioritizationModel);
    public Task UpdateJobPrioritizationAsync(JobPrioritizationModel jobPrioritizationModel);
    public Task DeleteJobPrioritizationAsync(Guid jobPrioritizationId);
    public Task<JobPrioritizationModel> GetJobPrioritizationByIdAsync(Guid id);
}