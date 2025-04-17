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

    public async Task DeleteAuditJobAsync(Guid auditJobId)
    {
        try 
        {
            var entity = await repository.GetByIdAsync(auditJobId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditJob with ID {auditJobId} not found.");
            }

            await repository.DeleteAsync(auditJobId);

            var cacheKey = string.Format(CacheKeys.AuditJob, auditJobId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete AuditJob.", ex);
        }
    }

    public Task<AuditJobModel> GetAuditJobByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAuditJobAsync(AuditJobModel auditJobModel)
    {
        ArgumentNullException.ThrowIfNull(auditJobModel, nameof(auditJobModel));

        try 
        {
            var entity = mapper.Map<AuditJob>(auditJobModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = string.Format(CacheKeys.AuditJob, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditJobTags,
                expiration: CacheExpirations.MediumTerm);
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update AuditJob.", ex);
        }
    }
}