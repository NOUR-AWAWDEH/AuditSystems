using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.UserDesignationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.UserDesignation;
using AuditSystem.Domain.Entities.Users;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.UserServices.UserDesignationServices;

internal sealed class UserDesignationService(
    IRepository<Guid, UserDesignation> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<UserDesignationService> logger) : 
    BaseEntityCacheService<UserDesignation>(
        cacheService,
        logger,
        CacheKeys.UserDesignation,
        UserDesignationTags,
        ListTags
    ),
    IUserDesignationService
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

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for UserDesignationModel");
            throw new ArgumentException("Invalid data mapping occurred", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create User Designation");
            throw new InvalidOperationException("Failed to create User Designation.", ex);
        }
    }

    public async Task DeleteUserDesignationAsync(Guid userDesignationId)
    {
        var cacheKey = GetCacheKey(userDesignationId);
        
        try
        {
            var entity = await repository.GetByIdAsync(userDesignationId)
                ?? throw new KeyNotFoundException($"UserDesignation with ID {userDesignationId} not found.");

            await repository.DeleteAsync(userDesignationId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "UserDesignation with ID {UserDesignationId} not found", userDesignationId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete UserDesignation ID {UserDesignationId}", userDesignationId);
            throw new InvalidOperationException($"Failed to delete UserDesignation: {userDesignationId}", ex);
        }
    }

    public async Task<UserDesignationModel> GetUserDesignationByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<UserDesignation>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<UserDesignationModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"User Designation with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<UserDesignationModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve User Designation with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve User Designation with ID '{id}'", ex);
        }
    }

    public async Task UpdateUserDesignationAsync(UserDesignationModel userDesignationModel)
    {
        ArgumentNullException.ThrowIfNull(userDesignationModel, nameof(userDesignationModel));
        
        try
        {
            var entity = mapper.Map<UserDesignation>(userDesignationModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update User Designation with ID {Id}", userDesignationModel.Id);
            throw new InvalidOperationException($"Failed to update User Designation with ID '{userDesignationModel.Id}'", ex);
        }
    }
}