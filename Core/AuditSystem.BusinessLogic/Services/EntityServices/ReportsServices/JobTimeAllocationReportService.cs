using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class JobTimeAllocationReportService(
    IRepository<Guid, JobTimeAllocationReport> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IJobTimeAllocationReportService
{
    private static readonly string[] JobTimeAllocationReportTags = ["job-time-allocation-reports", "job-time-allocation-report-list"];
    private static readonly string[] ListTags = ["job-time-allocation-report-list"]; // Tags for collections only

    public async Task<Guid> CreateJobTimeAllocationReportAsync(JobTimeAllocationReportModel jobTimeAllocationReportModel)
    {
        ArgumentNullException.ThrowIfNull(jobTimeAllocationReportModel, nameof(jobTimeAllocationReportModel));

        try
        {
            var entity = mapper.Map<JobTimeAllocationReport>(jobTimeAllocationReportModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.JobTimeAllocationReport, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: JobTimeAllocationReportTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create JobTimeAllocationReport.", ex);
        }
    }

    public async Task DeleteJobTimeAllocationReportAsync(Guid jobTimeAllocationReportId)
    {
        try 
        {
            var entity = await repository.GetByIdAsync(jobTimeAllocationReportId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"JobTimeAllocationReport with ID {jobTimeAllocationReportId} not found.");
            }

            await repository.DeleteAsync(jobTimeAllocationReportId);

            var cacheKey = string.Format(CacheKeys.JobTimeAllocationReport, jobTimeAllocationReportId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(JobTimeAllocationReportTags);
        }
        catch (Exception ex) 
        {
            throw new Exception("Failed to delete JobTimeAllocationReport.", ex);
        }
        
    }

    public Task<JobTimeAllocationReportModel> GetJobTimeAllocationReportByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateJobTimeAllocationReportAsync(JobTimeAllocationReportModel jobTimeAllocationReportModel)
    {
        ArgumentNullException.ThrowIfNull(jobTimeAllocationReportModel, nameof(jobTimeAllocationReportModel));
        
        try
        {
            var entity = mapper.Map<JobTimeAllocationReport>(jobTimeAllocationReportModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = string.Format(CacheKeys.JobTimeAllocationReport, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: JobTimeAllocationReportTags,
                expiration: CacheExpirations.MediumTerm);
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update JobTimeAllocationReport.", ex);
        }
    }
}
