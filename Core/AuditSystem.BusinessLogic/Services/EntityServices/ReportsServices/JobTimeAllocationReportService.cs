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
}
