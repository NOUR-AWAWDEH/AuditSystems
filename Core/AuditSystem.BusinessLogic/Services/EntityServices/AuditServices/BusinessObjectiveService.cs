using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class BusinessObjectiveService(
    IRepository<Guid, BusinessObjective> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<BusinessObjectiveService> logger) : 
    UnifiedCacheService<Guid, BusinessObjective, BusinessObjectiveModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.BusinessObjective,
        BusinessObjectiveTags,
        ListTags),
    IBusinessObjectiveService
{
    private static readonly string[] BusinessObjectiveTags = ["business-objectives", "business-objective-list"];
    private static readonly string[] ListTags = ["business-objective-list"];
    
    public async Task<Guid> CreateBusinessObjectiveAsync(BusinessObjectiveModel businessObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(businessObjectiveModel, nameof(businessObjectiveModel));

        var entity = mapper.Map<BusinessObjective>(businessObjectiveModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey,entity);

        return createdEntity.Id;
    }
    
    public async Task DeleteBusinessObjectiveAsync(Guid businessObjectiveId)
    {
        var cacheKey = GetCacheKey(businessObjectiveId);
       
        var entity = await repository.GetByIdAsync(businessObjectiveId)
                ?? throw new KeyNotFoundException($"BusinessObjective with ID {businessObjectiveId} not found.");
        
        await repository.DeleteAsync(businessObjectiveId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<BusinessObjectiveModel> GetBusinessObjectiveByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
      
        var cachedEntity = await cacheService.GetAsync<BusinessObjective>(cacheKey);
        if (cachedEntity != null)
        {
            return mapper.Map<BusinessObjectiveModel>(cachedEntity);
        }

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"BusinessObjective with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<BusinessObjectiveModel>(entity);
    }
    
    public Task<PaginationResult<BusinessObjectiveModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateBusinessObjectiveAsync(BusinessObjectiveModel businessObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(businessObjectiveModel, nameof(businessObjectiveModel));
        
        var entity = mapper.Map<BusinessObjective>(businessObjectiveModel);
        await repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
