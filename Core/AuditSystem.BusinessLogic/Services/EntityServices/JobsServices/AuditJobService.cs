using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Jobs;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.JobsServices;

internal sealed class AuditJobService(
    IRepository<Guid, AuditJob> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditJobService> logger) :
    BaseEntityCacheService<AuditJob>(
        cacheService,
        logger,
        CacheKeys.AuditJob,
        AuditJobTags,
        ListTags
    ), 
    IAuditJobService
{
    private static readonly string[] AuditJobTags = ["audit-jobs", "audit-job-list"];
    private static readonly string[] ListTags = ["audit-job-list"];

    public async Task<Guid> CreateAuditJobAsync(AuditJobModel auditJobModel)
    {
        ArgumentNullException.ThrowIfNull(auditJobModel, nameof(auditJobModel));

        try
        {
            var entity = mapper.Map<AuditJob>(auditJobModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, entity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for AuditJobModel");
            throw new ArgumentException("Mapping failed for AuditJobModel", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating the AuditJob.");
            throw new InvalidOperationException("An error occurred while creating the AuditJob.", ex);
        } 
    }
    
    public async Task DeleteAuditJobAsync(Guid auditJobId)
    {
        var cacheKey = GetCacheKey(auditJobId);
        
        try
        {
            var entity = await repository.GetByIdAsync(auditJobId)
                ?? throw new KeyNotFoundException($"AuditJob with ID {auditJobId} not found.");

            await repository.DeleteAsync(auditJobId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "AuditJob with ID {AuditJobId} not found", auditJobId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete AuditJob ID {AuditJobId}", auditJobId);
            throw new InvalidOperationException($"Failed to delete AuditJob: {auditJobId}", ex);
        }
    }
    
    public async Task<AuditJobModel> GetAuditJobByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<AuditJob>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditJobModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"AuditJob with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<AuditJobModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve AuditJob with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve AuditJob with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<AuditJobModel>> SearchAsync(string? name, Page page)
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
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update AuditJob with ID {Id}", auditJobModel.Id);
            throw new InvalidOperationException($"Failed to update AuditJob with ID '{auditJobModel.Id}'", ex);
        }
    }
}