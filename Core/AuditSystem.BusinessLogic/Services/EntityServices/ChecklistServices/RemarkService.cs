using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Domain.Entities.Checklists;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ChecklistServices;

internal sealed class RemarkService(
    IRepository<Guid, Remark> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RemarkService> logger) :
    BaseEntityCacheService<Remark>(
        cacheService,
        logger,
        CacheKeys.Remark,
        RemarkTags,
        ListTags
    ),
    IRemarkService
{
    private static readonly string[] RemarkTags = ["remarks", "remark-list"];
    private static readonly string[] ListTags = ["remark-list"];
    
    public async Task<Guid> CreateRemarkAsync(RemarkModel remarkModel)
    {
        ArgumentNullException.ThrowIfNull(remarkModel, nameof(remarkModel));

        try
        {
            var entity = mapper.Map<Remark>(remarkModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, entity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for RemarkModel");
            throw new ArgumentException("Mapping failed for RemarkModel", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while creating Remark.");
            throw new InvalidOperationException("An unexpected error occurred while creating Remark.", ex);
        }
    }
    
    public async Task DeleteRemarkAsync(Guid remarkId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(remarkId);
            if (entity is null)
            {
                throw new KeyNotFoundException($"Remark with ID {remarkId} not found.");
            }

            await repository.DeleteAsync(remarkId);

            var cacheKey = string.Format(CacheKeys.Remark, remarkId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete Remark.", ex);
        }

    }
    
    public async Task<RemarkModel> GetRemarkByIdAsync(Guid id)
    {
       try
       {
            var cacheKey = string.Format(CacheKeys.Remark, id);

            var cachedEntity = await cacheService.GetAsync<Remark>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<RemarkModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Remark with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: RemarkTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<RemarkModel>(entity);  
       }
       catch (Exception ex)
       {
            throw new Exception($"Failed to get Remark ID {id}.", ex);
       }
    }
    
    public async Task UpdateRemarkAsync(RemarkModel remarkModel)
    {
        ArgumentNullException.ThrowIfNull(remarkModel, nameof(remarkModel));

        try
        {
            var entity = mapper.Map<Remark>(remarkModel);

            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.Remark, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: RemarkTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update Remark.", ex);
        }
    }
}