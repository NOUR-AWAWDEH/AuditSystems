using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class SpecialProjectService(
    IRepository<Guid, SpecialProject> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISpecialProjectService
{
    private static readonly string[] SpecialProjectTags = ["special-projects", "special-project-list"];
    private static readonly string[] ListTags = ["special-project-list"]; // Tags for collections only

    public async Task<Guid> CreateSpecialProjectAsync(SpecialProjectModel specialProjectModel)
    {
        ArgumentNullException.ThrowIfNull(specialProjectModel, nameof(specialProjectModel));

        try
        {
            var entity = mapper.Map<SpecialProject>(specialProjectModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SpecialProject, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: SpecialProjectTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create SpecialProject.", ex);
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