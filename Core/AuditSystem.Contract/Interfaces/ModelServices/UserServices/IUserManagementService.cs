using AuditSystem.Contract.Models.Users;

namespace AuditSystem.Contract.Interfaces.ModelServices.UserServices;

public interface IUserManagementService
{
    public Task<Guid> CreateUserAsync(UserManagementModel userManagementModel);
}
