using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;

internal sealed class AuditProcessService(
    IRepository<Guid, AuditProcess> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditProcessService
{
    private static readonly string[] AuditProcessTags = ["audit-processes", "audit-process-list"];
    private static readonly string[] ListTags = ["audit-process-list"]; // Tags for collections only

    public async Task<Guid> CreateAuditProcessAsync(AuditProcessModel processModel)
    {
        ArgumentNullException.ThrowIfNull(processModel, nameof(processModel));

        try
        {
            var entity = mapper.Map<AuditProcess>(processModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditProcess, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: AuditProcessTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create AuditProcess.", ex);
        }
    }
    public async Task DeleteAuditProcessAsync(Guid auditProcessId)
    {
        try 
        {
            var entity = await repository.GetByIdAsync(auditProcessId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditProcess with ID {auditProcessId} not found.");
            }

            await repository.DeleteAsync(auditProcessId);

            var cacheKey = string.Format(CacheKeys.AuditProcess, auditProcessId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete AuditProcess.", ex);
        }
    }
    public async Task<AuditProcessModel> GetAuditProcessByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.AuditProcess, id);

            var cachedEntity = await cacheService.GetAsync<AuditProcess>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditProcessModel>(cachedEntity); 
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditProcess with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditProcessTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<AuditProcessModel>(entity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get AuditProcess ID {id}.", ex);
        }
    }
    public async Task UpdateAuditProcessAsync(AuditProcessModel auditProcessModel)
    {
        ArgumentNullException.ThrowIfNull(auditProcessModel, nameof(auditProcessModel));

        try
        {
            var entity = mapper.Map<AuditProcess>(auditProcessModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = string.Format(CacheKeys.AuditProcess, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditProcessTags,
                expiration: CacheExpirations.MediumTerm);
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update AuditProcess.", ex);
        }
    }
}
