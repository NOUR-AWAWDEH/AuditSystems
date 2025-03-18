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
    public async Task<Guid> CreateUserAsync(UserManagementModel userManagementModel)
    {
        ArgumentNullException.ThrowIfNull(userManagementModel, nameof(userManagementModel));

        try
        {
            var entity = mapper.Map<UserManagement>(userManagementModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}