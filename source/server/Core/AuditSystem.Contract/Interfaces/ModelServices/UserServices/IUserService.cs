using AuditSystem.Contract.Models.Users;

namespace AuditSystem.Contract.Interfaces.ModelServices.UserServices;

public interface IUserService
{
    Task<List<UserModel>> GetActionOwnersByUserIdAsync(Guid userId);
}
