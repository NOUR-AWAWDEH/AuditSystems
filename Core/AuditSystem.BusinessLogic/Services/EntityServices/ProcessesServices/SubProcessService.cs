using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;

internal sealed class SubProcessService(
    IRepository<Guid, SubProcess> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SubProcessService> logger) :
    UnifiedCacheService<Guid, SubProcess, SubProcessModel>(
        repository,
        mapper,
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
       
        var entity = mapper.Map<SubProcess>(subProcessModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteSubProcessAsync(Guid subProcessId)
    {
        var cacheKey = GetCacheKey(subProcessId);
        
        var entity = await repository.GetByIdAsync(subProcessId)
            ?? throw new KeyNotFoundException($"SubProcess with ID {subProcessId} not found.");

        await repository.DeleteAsync(subProcessId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<SubProcessModel> GetSubProcessByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<SubProcess>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<SubProcessModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"SubProcess with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<SubProcessModel>(entity);
    }

    public Task<PaginationResult<SubProcessModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateSubProcessAsync(SubProcessModel subProcessModel)
    {
        ArgumentNullException.ThrowIfNull(subProcessModel, nameof(subProcessModel));

        var entity = mapper.Map<SubProcess>(subProcessModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
