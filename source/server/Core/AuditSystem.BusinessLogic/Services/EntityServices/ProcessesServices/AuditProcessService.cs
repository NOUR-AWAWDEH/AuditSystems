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

internal sealed class AuditProcessService(
    IRepository<Guid, AuditProcess> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditProcessService> logger) : 
    UnifiedCacheService<Guid, AuditProcess, AuditProcessModel>(
        repository,
        mapper,
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

        var entity = mapper.Map<AuditProcess>(processModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteAuditProcessAsync(Guid auditProcessId)
    {
        var cacheKey = GetCacheKey(auditProcessId);
        
        
        var entity = await repository.GetByIdAsync(auditProcessId)
            ?? throw new KeyNotFoundException($"AuditProcess with ID {auditProcessId} not found.");

        await repository.DeleteAsync(auditProcessId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<AuditProcessModel> GetAuditProcessByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
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

    public async Task<PaginationResult<AuditProcessModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<AuditProcessModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<AuditProcessModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateAuditProcessAsync(AuditProcessModel auditProcessModel)
    {
        ArgumentNullException.ThrowIfNull(auditProcessModel, nameof(auditProcessModel));

        var entity = mapper.Map<AuditProcess>(auditProcessModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
