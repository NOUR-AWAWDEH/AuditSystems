using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Domain.Entities.Common;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.CommonServices;

internal sealed class RatingService(
    IRepository<Guid, Rating> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IRatingService
{
    private static readonly string[] RatingTags = ["ratings", "rating-list"];
    private static readonly string[] ListTags = ["rating-list"]; // Tags for collections only

    public async Task<Guid> CreateRatingAsync(RatingModel ratingModel)
    {
        ArgumentNullException.ThrowIfNull(ratingModel, nameof(ratingModel));

        try
        {
            var entity = mapper.Map<Rating>(ratingModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.Rating, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: RatingTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create Rating.", ex);
        }
    }

    public async Task DeleteRatingAsync(Guid ratingId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(ratingId);
            if (entity is null)
            {
                throw new KeyNotFoundException($"Rating with ID {ratingId} not found.");
            }
            
            await repository.DeleteAsync(ratingId);
            
            var cacheKey = string.Format(CacheKeys.Rating, ratingId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete Rating.", ex);
        }
    }

    public async Task UpdateRatingAsync(RatingModel ratingModel)
    {
        ArgumentNullException.ThrowIfNull(ratingModel, nameof(ratingModel));

        try 
        {
            var entity = mapper.Map<Rating>(ratingModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.Rating, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: RatingTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update Rating.", ex);
        }
    }
}
