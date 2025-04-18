using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;

internal sealed class SubProcessService(
    IRepository<Guid, SubProcess> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISubProcessService
{
    private static readonly string[] SubProcessTags = ["sub-processes", "sub-process-list"];
    private static readonly string[] ListTags = ["sub-process-list"]; // Tags for collections only

    public async Task<Guid> CreateSubProcessAsync(SubProcessModel subProcessModel)
    {
        ArgumentNullException.ThrowIfNull(subProcessModel, nameof(subProcessModel));

        try
        {
            var entity = mapper.Map<SubProcess>(subProcessModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SubProcess, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: SubProcessTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create SubProcess.", ex);
        }
    }
    public async Task DeleteSubProcessAsync(Guid subProcessId)
    {
        try 
        {
            var entity = await repository.GetByIdAsync(subProcessId);
            if (entity == null)
            {
                throw new Exception("SubProcess not found.");
            }

            await repository.DeleteAsync(subProcessId);

            var cacheKey = string.Format(CacheKeys.SubProcess, subProcessId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete SubProcess.", ex);
        }
    }
    public async Task<SubProcessModel> GetSubProcessByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.SubProcess, id);

            var cachedSubProcess = await cacheService.GetAsync<SubProcess>(cacheKey);
            if (cachedSubProcess != null)
            {
                return mapper.Map<SubProcessModel>(cachedSubProcess);
            }

            var entity = await repository.GetByIdAsync(id);

            if (entity == null)
            {
               throw new KeyNotFoundException($"SubProcess with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: SubProcessTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<SubProcessModel>(entity); 
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get SubProcess by ID {id}.", ex);
        }
    }
    public async Task UpdateSubProcessAsync(SubProcessModel subProcessModel)
    {
        ArgumentNullException.ThrowIfNull(subProcessModel, nameof(subProcessModel));

        try
        {
            var entity = mapper.Map<SubProcess>(subProcessModel);
            
            await repository.UpdateAsync(entity);
            
            var cacheKey = string.Format(CacheKeys.SubProcess, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: SubProcessTags,
                expiration: CacheExpirations.MediumTerm);
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update SubProcess.", ex);
        }
    }
}
