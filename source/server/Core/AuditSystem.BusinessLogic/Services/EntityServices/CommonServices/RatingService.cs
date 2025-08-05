using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Common;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.CommonServices;

internal sealed class RatingService(
    IRepository<Guid, Rate> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RatingService> logger) :
    UnifiedCacheService<Guid, Rate, RateModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.Rate,
        RatingTags,
        ListTags
    ), 
    IRateService
{
    private static readonly string[] RatingTags = ["ratings", "rating-list"];
    private static readonly string[] ListTags = ["rating-list"];

    public async Task<Guid> CreateRatingAsync(RateModel ratingModel)
    {
        ArgumentNullException.ThrowIfNull(ratingModel, nameof(ratingModel));

        var entity = mapper.Map<Rate>(ratingModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, entity);

        return createdEntity.Id;
    }
    
    public async Task DeleteRatingAsync(Guid ratingId)
    {
        var cacheKey = GetCacheKey(ratingId);
        
        var entity = await repository.GetByIdAsync(ratingId)
            ?? throw new KeyNotFoundException($"Rating with ID {ratingId} not found.");

        await repository.DeleteAsync(ratingId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<RateModel> GetRatingByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
       
        var cachedEntity = await cacheService.GetAsync<Rate>(cacheKey);
        if (cachedEntity is not null)
            return mapper.Map<RateModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Rating with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<RateModel>(entity);
    }

    public async Task<PaginationResult<RateModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<RateModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<RateModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateRatingAsync(RateModel ratingModel)
    {
        ArgumentNullException.ThrowIfNull(ratingModel, nameof(ratingModel));

        var entity = mapper.Map<Rate>(ratingModel);
        await repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
