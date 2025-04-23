using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Domain.Entities.Skills;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SkillsServices;

internal sealed class SkillSetService(
    IRepository<Guid, SkillSet> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SkillSetService> logger) : 
    BaseEntityCacheService<SkillSet>(
        cacheService,
        logger,
        CacheKeys.SkillSet,
        SkillSetTags,
        ListTags
    ),
    ISkillSetService
{
    private static readonly string[] SkillSetTags = ["skill-sets", "skill-set-list"];
    private static readonly string[] ListTags = ["skill-set-list"];

    public async Task<Guid> CreateSkillSetAsync(SkillSetModel skillSetModel)
    {
        ArgumentNullException.ThrowIfNull(skillSetModel, nameof(skillSetModel));

        try
        {
            var entity = mapper.Map<SkillSet>(skillSetModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for SkillSetModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create SkillSet");
            throw new InvalidOperationException("Failed to create SkillSet.", ex);
        }
    }

    public async Task DeleteSkillSetAsync(Guid skillSetId)
    {
        var cacheKey = GetCacheKey(skillSetId);
        
        try
        {
            var entity = await repository.GetByIdAsync(skillSetId)
                ?? throw new KeyNotFoundException($"SkillSet with ID {skillSetId} not found.");

            await repository.DeleteAsync(skillSetId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "SkillSet with ID {SkillSetId} not found", skillSetId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete SkillSet ID {SkillSetId}", skillSetId);
            throw new InvalidOperationException($"Failed to delete SkillSet: {skillSetId}", ex);
        }
    }

    public async Task<SkillSetModel> GetSkillSetByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<SkillSet>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<SkillSetModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"SkillSet with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<SkillSetModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve SkillSet with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve SkillSet with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<SkillSetModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateSkillSetAsync(SkillSetModel skillSetModel)
    {
        ArgumentNullException.ThrowIfNull(skillSetModel, nameof(skillSetModel));
        
        try
        {
            var entity = mapper.Map<SkillSet>(skillSetModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update SkillSet with ID {Id}", skillSetModel.Id);
            throw new InvalidOperationException($"Failed to update SkillSet with ID '{skillSetModel.Id}'", ex);
        }
    }
}