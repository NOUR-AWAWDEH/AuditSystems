using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;

public interface IAppUserValidation
{
    Task<AppUser> ValidateAdminOwnsUser(Guid userId, Guid adminUserId);
}