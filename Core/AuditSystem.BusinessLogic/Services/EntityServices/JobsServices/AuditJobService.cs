using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Domain.Entities.Jobs;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.JobsServices;

internal sealed class AuditJobService(
    IRepository<Guid, AuditJob> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditJobService
{
    private static readonly string[] AuditJobTags = ["audit-jobs", "audit-job-list"];
    private static readonly string[] ListTags = ["audit-job-list"]; // Tags for collections only

    public async Task<Guid> CreateAuditJobAsync(AuditJobModel auditJobModel)
    {
        ArgumentNullException.ThrowIfNull(auditJobModel, nameof(auditJobModel));

        try
        {
            var entity = mapper.Map<AuditJob>(auditJobModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditJob, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: AuditJobTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create AuditJob.", ex);
        }
    }
}
