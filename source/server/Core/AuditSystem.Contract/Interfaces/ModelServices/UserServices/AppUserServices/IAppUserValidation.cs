using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;

public interface IAppUserValidation
{
    Task<AppUser> ValidateAdminOwnsUser(Guid userId, Guid adminUserId); //done
    Task<ApplicationUser> ValidateIdentityUserExists(Guid userId); //done
    Task<AppUser> ValidateAppUserExists(Guid userId); //done
    //Task<AppUser?> GetAppUserByIdentityUserId(Guid identityUserId);
}