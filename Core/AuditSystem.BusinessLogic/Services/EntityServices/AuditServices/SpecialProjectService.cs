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

internal sealed class SpecialProjectService(
    IRepository<Guid, SpecialProject> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SpecialProjectService> logger) :
    BaseEntityCacheService<SpecialProject>(
        cacheService,
        logger,
        CacheKeys.SpecialProject,
        SpecialProjectTags,
        ListTags 
    ), 
    ISpecialProjectService
{
    private static readonly string[] SpecialProjectTags = ["special-projects", "special-project-list"];
    private static readonly string[] ListTags = ["special-project-list"];
    
    public async Task<Guid> CreateSpecialProjectAsync(SpecialProjectModel specialProjectModel)
    {
        ArgumentNullException.ThrowIfNull(specialProjectModel, nameof(specialProjectModel));

        try
        {
            var entity = mapper.Map<SpecialProject>(specialProjectModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, entity);

            return createdEntity.Id;
        }
        catch( AutoMapperMappingException ex )
        {
            logger.LogError(ex, "Mapping with SpecialProjectModel to SpecialProject failed.");
            throw new ArgumentException("AutoMapper mapping exception occurred while creating SpecialProjectModel.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create SpecialProject.");
            throw new InvalidOperationException("Failed to create SpecialProject.", ex);
        }
    }
    
    public async Task DeleteSpecialProjectAsync(Guid specialProjectId)
    {
        var cacheKey = GetCacheKey(specialProjectId);

        try 
        {
            var entity = await repository.GetByIdAsync(specialProjectId)
                ?? throw new KeyNotFoundException($"SpecialProject with ID {specialProjectId} not found.");    
            
            await repository.DeleteAsync(specialProjectId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "SpecialProject with ID: {id} not found.",specialProjectId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete SpecialProject with ID: {id}.", specialProjectId);
            throw new InvalidOperationException("Failed to delete SpecialProject.", ex);
        }
    }
    
    public async Task<SpecialProjectModel> GetSpecialProjectByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<SpecialProject>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<SpecialProjectModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"SpecialProject with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<SpecialProjectModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve SpecialProject with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve SpecialProject with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<SpecialProjectModel>> Search(string? Name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateSpecialProjectAsync(SpecialProjectModel specialProjectModel)
    {
        ArgumentNullException.ThrowIfNull(specialProjectModel, nameof(specialProjectModel));

        try
        {
            var entity = mapper.Map<SpecialProject>(specialProjectModel);
            await repository.UpdateAsync(entity);

            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update SpecialProject with ID {Id}", specialProjectModel.Id);
            throw new InvalidOperationException($"Failed to update SpecialProject with ID '{specialProjectModel.Id}'", ex);
        }
    }
}