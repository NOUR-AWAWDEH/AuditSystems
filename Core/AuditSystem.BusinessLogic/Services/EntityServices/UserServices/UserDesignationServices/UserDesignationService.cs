using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.UserDesignationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.UserDesignation;
using AuditSystem.Domain.Entities.Users;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.UserServices.UserDesignationServices;

internal sealed class UserDesignationService(
    IRepository<Guid, UserDesignation> repository,
    IMapper mapper,
    ICacheService cacheService
    ) : IUserDesignationService
{
    private static readonly string[] UserDesignationTags = ["user-designations", "user-designation-list"];
    private static readonly string[] ListTags = ["user-designation-list"];

    public async Task<Guid> CreateUserDesignationAsync(UserDesignationModel userDesignationModel)
    {
        ArgumentNullException.ThrowIfNull(userDesignationModel, nameof(userDesignationModel));

        try
        {
            var entity = mapper.Map<UserDesignation>(userDesignationModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.UserDesignation, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: UserDesignationTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create User Designation.", ex);
        }
    }

    public async Task DeleteUserDesignationAsync(Guid userDesignationId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(userDesignationId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"UserDesignation with ID {userDesignationId} not found.");
            }

            await repository.DeleteAsync(userDesignationId);

            var cacheKey = string.Format(CacheKeys.UserDesignation, userDesignationId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete User Designation.", ex);
        }
    }

    public async Task<UserDesignationModel> GetUserDesignationByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.UserDesignation, id);

            var cachedEntity = await cacheService.GetAsync<UserDesignation>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<UserDesignationModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"User Designation with ID {id} not found."); 
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: UserDesignationTags,
                expiration: CacheExpirations.MediumTerm
            ); 
            
            return mapper.Map<UserDesignationModel>(entity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get User Designation by ID {id}.", ex);
        }
    }

    public async Task UpdateUserDesignationAsync(UserDesignationModel userDesignationModel)
    {
        ArgumentNullException.ThrowIfNull(userDesignationModel, nameof(userDesignationModel));

        try
        {
            var entity = mapper.Map<UserDesignation>(userDesignationModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = string.Format(CacheKeys.UserDesignation.ToString(), entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: UserDesignationTags,
                expiration: CacheExpirations.MediumTerm);
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update User Designation.", ex);
        }
    }
}