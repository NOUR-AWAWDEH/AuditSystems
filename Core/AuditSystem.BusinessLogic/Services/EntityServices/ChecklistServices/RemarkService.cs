using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Checklists;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ChecklistServices;

internal sealed class RemarkService(
    IRepository<Guid, Remark> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RemarkService> logger) :
    UnifiedCacheService<Guid, Remark, RemarkModel>(
        repository,
        mapper,
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

        var entity = mapper.Map<Remark>(remarkModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, entity);

        return createdEntity.Id;
    }
    
    public async Task DeleteRemarkAsync(Guid remarkId)
    {
        var cacheKey = GetCacheKey(remarkId);

        var entity = await repository.GetByIdAsync(remarkId) 
            ?? throw new KeyNotFoundException($"Remark with ID {remarkId} not found.");

        await repository.DeleteAsync(remarkId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<RemarkModel> GetRemarkByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<Remark>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<RemarkModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Remark with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<RemarkModel>(entity);
    }

    public Task<PaginationResult<RemarkModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateRemarkAsync(RemarkModel remarkModel)
    {
        ArgumentNullException.ThrowIfNull(remarkModel, nameof(remarkModel));
        
        var entity = mapper.Map<Remark>(remarkModel);
        await repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
}
}