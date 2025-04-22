using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditUniverseObjectiveService(
    IRepository<Guid, AuditUniverseObjective> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditUniverseObjectiveService> logger) :
    BaseEntityCacheService<AuditUniverseObjective>
    (
        cacheService,
        logger,
        CacheKeys.AuditUniverseObjective,
        AuditUniverseObjectiveTags,
        ListTags
    ),
    IAuditUniverseObjectiveService

{
    private static readonly string[] AuditUniverseObjectiveTags = ["audit-universe-objectives", "audit-universe-objective-list"];
    private static readonly string[] ListTags = ["audit-universe-objective-list"];

    public async Task<Guid> CreateAuditUniverseObjectiveAsync(AuditUniverseObjectiveModel auditUniverseObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseObjectiveModel, nameof(auditUniverseObjectiveModel));

        try
        {
            var entity = mapper.Map<AuditUniverseObjective>(auditUniverseObjectiveModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, entity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for AuditUniverseObjectiveModel");
            throw new ArgumentException("Invalid data mapping occurred", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create AuditUniverseObjective");
            throw new InvalidCastException("Failed to create AuditUniverseObjective.", ex);
        }
    }

    public async Task DeleteAuditUniverseObjectiveAsync(Guid auditUniverseObjectiveId)
    {
        var cacheKey = GetCacheKey(auditUniverseObjectiveId);
        
        try
        {
            var entity = await repository.GetByIdAsync(auditUniverseObjectiveId)
                ?? throw new KeyNotFoundException($"AuditUniverseObjective with ID {auditUniverseObjectiveId} not found.");

            await repository.DeleteAsync(auditUniverseObjectiveId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("AuditUniverseObjective with ID {AuditUniverseObjectiveId} not found.", auditUniverseObjectiveId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete AuditUniverseObjective with ID {AuditUniverseObjectiveId}", auditUniverseObjectiveId);
            throw new Exception("Failed to delete AuditUniverseObjective.", ex);
        }
    }

    public async Task<AuditUniverseObjectiveModel> GetAuditUniverseObjectiveByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);

        try
        {
            var cachedEntity = await cacheService.GetAsync<AuditUniverseObjective>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditUniverseObjectiveModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"AuditUniverseObjective with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);

            return mapper.Map<AuditUniverseObjectiveModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve AuditUniverseObjective with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve AuditUniverseObjective with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<AuditUniverseObjectiveModel>> Search(string? Name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAuditUniverseObjectiveAsync(AuditUniverseObjectiveModel auditUniverseObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseObjectiveModel, nameof(auditUniverseObjectiveModel));
        
        try
        {
            var entity = mapper.Map<AuditUniverseObjective>(auditUniverseObjectiveModel);
            await repository.UpdateAsync(entity);

            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update AuditUniverseObjective with ID {Id}", auditUniverseObjectiveModel.Id);
            throw new InvalidOperationException($"Failed to update AuditUniverseObjective with ID '{auditUniverseObjectiveModel.Id}'", ex);
        }
    }
}
