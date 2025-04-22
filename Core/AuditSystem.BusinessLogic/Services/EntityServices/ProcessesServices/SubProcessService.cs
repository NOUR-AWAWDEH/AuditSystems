using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;

internal sealed class SubProcessService(
    IRepository<Guid, SubProcess> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SubProcessService> logger) :
    BaseEntityCacheService<SubProcess>(
        cacheService,
        logger,
        CacheKeys.SubProcess,
        SubProcessTags,
        ListTags
    ), 
    ISubProcessService
{
    private static readonly string[] SubProcessTags = ["sub-processes", "sub-process-list"];
    private static readonly string[] ListTags = ["sub-process-list"];

    public async Task<Guid> CreateSubProcessAsync(SubProcessModel subProcessModel)
    {
        ArgumentNullException.ThrowIfNull(subProcessModel, nameof(subProcessModel));

        try
        {
            var entity = mapper.Map<SubProcess>(subProcessModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for SubProcessModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create SubProcess");
            throw new InvalidOperationException("Failed to create SubProcess.", ex);
        }
    }
    
    public async Task DeleteSubProcessAsync(Guid subProcessId)
    {
        var cacheKey = GetCacheKey(subProcessId);
        
        try
        {
            var entity = await repository.GetByIdAsync(subProcessId)
                ?? throw new KeyNotFoundException($"SubProcess with ID {subProcessId} not found.");

            await repository.DeleteAsync(subProcessId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "SubProcess with ID {SubProcessId} not found", subProcessId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete SubProcess ID {SubProcessId}", subProcessId);
            throw new InvalidOperationException($"Failed to delete SubProcess: {subProcessId}", ex);
        }
    }
    
    public async Task<SubProcessModel> GetSubProcessByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<SubProcess>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<SubProcessModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"SubProcess with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<SubProcessModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve SubProcess with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve SubProcess with ID '{id}'", ex);
        }
    }
    
    public async Task UpdateSubProcessAsync(SubProcessModel subProcessModel)
    {
        ArgumentNullException.ThrowIfNull(subProcessModel, nameof(subProcessModel));

        try
        {
            var entity = mapper.Map<SubProcess>(subProcessModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update SubProcess with ID {Id}", subProcessModel.Id);
            throw new InvalidOperationException($"Failed to update SubProcess with ID '{subProcessModel.Id}'", ex);
        }
    }
}
