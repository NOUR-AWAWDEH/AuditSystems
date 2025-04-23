using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;

internal sealed class AuditProcessService(
    IRepository<Guid, AuditProcess> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditProcessService> logger) : 
    BaseEntityCacheService<AuditProcess>(
        cacheService,
        logger,
        CacheKeys.AuditProcess,
        AuditProcessTags,
        ListTags
    ),
    IAuditProcessService
{
    private static readonly string[] AuditProcessTags = ["audit-processes", "audit-process-list"];
    private static readonly string[] ListTags = ["audit-process-list"];

    public async Task<Guid> CreateAuditProcessAsync(AuditProcessModel processModel)
    {
        ArgumentNullException.ThrowIfNull(processModel, nameof(processModel));

        try
        {
            var entity = mapper.Map<AuditProcess>(processModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for AuditProcessModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create AuditProcess");
            throw new InvalidOperationException("Failed to create AuditProcess.", ex);
        }
    }
    
    public async Task DeleteAuditProcessAsync(Guid auditProcessId)
    {
        var cacheKey = GetCacheKey(auditProcessId);
        
        try
        {
            var entity = await repository.GetByIdAsync(auditProcessId)
                ?? throw new KeyNotFoundException($"AuditProcess with ID {auditProcessId} not found.");

            await repository.DeleteAsync(auditProcessId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "AuditProcess with ID {AuditProcessId} not found", auditProcessId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete AuditProcess with ID {AuditProcessId}", auditProcessId);
            throw new InvalidOperationException($"Failed to delete AuditProcess with ID {auditProcessId}", ex);
        }
    }
    
    public async Task<AuditProcessModel> GetAuditProcessByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<AuditProcess>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditProcessModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"AuditProcess with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<AuditProcessModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve AuditProcess with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve AuditProcess with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<AuditProcessModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAuditProcessAsync(AuditProcessModel auditProcessModel)
    {
        ArgumentNullException.ThrowIfNull(auditProcessModel, nameof(auditProcessModel));

        try
        {
            var entity = mapper.Map<AuditProcess>(auditProcessModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update AuditProcess with ID {Id}", auditProcessModel.Id);
            throw new InvalidOperationException($"Failed to update AuditProcess with ID '{auditProcessModel.Id}'", ex);
        }
    }
}
