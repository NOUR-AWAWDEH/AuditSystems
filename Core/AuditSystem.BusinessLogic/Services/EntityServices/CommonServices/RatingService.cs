using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Domain.Entities.Common;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.CommonServices;

internal sealed class RatingService(
    IRepository<Guid, Rating> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RatingService> logger) :
    BaseEntityCacheService<Rating>
    (
        cacheService,
        logger,
        CacheKeys.Rating,
        RatingTags,
        ListTags
    ), 
    IRatingService
{
    private static readonly string[] RatingTags = ["ratings", "rating-list"];
    private static readonly string[] ListTags = ["rating-list"];

    public async Task<Guid> CreateRatingAsync(RatingModel ratingModel)
    {
        ArgumentNullException.ThrowIfNull(ratingModel, nameof(ratingModel));

        try
        {
            var entity = mapper.Map<Rating>(ratingModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, entity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for RatingModel");
            throw new ArgumentException("Mapping failed for RatingModel", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create Rating.");
            throw new InvalidOperationException("Failed to create Rating.", ex);
        }
    }
    
    public async Task DeleteRatingAsync(Guid ratingId)
    {
        var cacheKey = GetCacheKey(ratingId);
        
        try
        {
            var entity = await repository.GetByIdAsync(ratingId)
                ?? throw new KeyNotFoundException($"Rating with ID {ratingId} not found.");

            await repository.DeleteAsync(ratingId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Rating with ID {RatingId} not found", ratingId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete Rating ID {RatingId}", ratingId);
            throw new InvalidOperationException($"Failed to delete rating: {ratingId}", ex);
        }
    }
    
    public async Task<RatingModel> GetRatingByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<Rating>(cacheKey);
            if (cachedEntity is not null)
            {
                return mapper.Map<RatingModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Rating with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<RatingModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve Rating with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve Rating with ID '{id}'", ex);
        }
    }

    public async Task UpdateRatingAsync(RatingModel ratingModel)
    {
        ArgumentNullException.ThrowIfNull(ratingModel, nameof(ratingModel));

        try 
        {
            var entity = mapper.Map<Rating>(ratingModel);
            await repository.UpdateAsync(entity);

            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update Rating with ID {Id}", ratingModel.Id);
            throw new InvalidOperationException($"Failed to update Rating with ID '{ratingModel.Id}'", ex);
        }
    }
}
