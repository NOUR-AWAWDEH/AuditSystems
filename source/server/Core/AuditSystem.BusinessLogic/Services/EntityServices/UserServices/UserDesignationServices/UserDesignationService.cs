using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.UserDesignationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.UserDesignation;
using AuditSystem.Domain.Entities.Users;
using Microsoft.Extensions.Logging;
using AutoMapper;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.UserServices.UserDesignationServices;

internal sealed class UserDesignationService(
    IRepository<Guid, UserDesignation> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<UserDesignationService> logger) : 
    UnifiedCacheService<Guid, UserDesignation, UserDesignationModel>
    (
        repository,
        mapper,
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
       
        var entity = mapper.Map<UserDesignation>(userDesignationModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteUserDesignationAsync(Guid userDesignationId)
    {
        var cacheKey = GetCacheKey(userDesignationId);
        
        var entity = await repository.GetByIdAsync(userDesignationId)
            ?? throw new KeyNotFoundException($"UserDesignation with ID {userDesignationId} not found.");

        await repository.DeleteAsync(userDesignationId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<UserDesignationModel> GetUserDesignationByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var entity = await cacheService.GetAsync<UserDesignation>(cacheKey);
        if (entity != null)
            return mapper.Map<UserDesignationModel>(entity);

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"User Designation with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<UserDesignationModel>(entity);
    }

    public async Task<PaginationResult<UserDesignationModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<UserDesignationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<UserDesignationModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateUserDesignationAsync(UserDesignationModel userDesignationModel)
    {
        ArgumentNullException.ThrowIfNull(userDesignationModel, nameof(userDesignationModel));
        
        var entity = mapper.Map<UserDesignation>(userDesignationModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}