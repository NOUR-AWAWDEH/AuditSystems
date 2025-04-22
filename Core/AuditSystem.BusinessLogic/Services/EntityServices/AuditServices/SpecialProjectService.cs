using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
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
        try 
        {
            var entity = await repository.GetByIdAsync(specialProjectId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"SpecialProject with ID {specialProjectId} not found.");
            }
            
            await repository.DeleteAsync(specialProjectId);
            
            var cacheKey = string.Format(CacheKeys.SpecialProject, specialProjectId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete SpecialProject.", ex);
        }
    }
    
    public async Task<SpecialProjectModel> GetSpecialProjectByIdAsync(Guid id)
    {
       try
       {
           var cacheKey = string.Format(CacheKeys.SpecialProject, id);

           var cachedEntity = await cacheService.GetAsync<SpecialProject>(cacheKey);

           if (cachedEntity != null)
           {
               return mapper.Map<SpecialProjectModel>(cachedEntity);
           }

           var entity = await repository.GetByIdAsync(id);
           if (entity == null)
           {
               throw new KeyNotFoundException($"SpecialProject with ID {id} not found.");
           }

           await cacheService.SetAsync(
               key: cacheKey,
               value: entity,
               tags: SpecialProjectTags,
               expiration: CacheExpirations.MediumTerm  
           );

           return mapper.Map<SpecialProjectModel>(entity);
       }
       catch (Exception ex)
       {
           throw new Exception($"Failed to get SpecialProject with ID {id}.", ex);
       }
    }
    
    public async Task UpdateSpecialProjectAsync(SpecialProjectModel specialProjectModel)
    {
        ArgumentNullException.ThrowIfNull(specialProjectModel, nameof(specialProjectModel));

        try
        {
            var entity = mapper.Map<SpecialProject>(specialProjectModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SpecialProject, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: SpecialProjectTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update SpecialProject.", ex);
        }
    }
}