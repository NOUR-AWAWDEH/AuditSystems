using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Domain.Entities.Users;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.UserServices;

internal sealed class UserManagementService(
    IRepository<Guid, UserManagement> repository,
    IMapper mapper,
    ICacheService cacheService) 
    : IUserManagementService
{
    private static readonly string[] UserManagementTags = ["user-managements", "user-management-list"];
    private static readonly string[] ListTags = ["user-management-list"]; // Tags for collections only

    public async Task<Guid> CreateUserAsync(UserManagementModel userManagementModel)
    {
        ArgumentNullException.ThrowIfNull(userManagementModel, nameof(userManagementModel));

        try
        {
            var entity = mapper.Map<UserManagement>(userManagementModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.CacheKey, CacheKeys.UserManagement, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: UserManagementTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create UserManagement.", ex);
        }
    }
}