using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.UserHelpers.AppUserHelpers;

public class AppUserValidationHelper(
    IRepository<Guid, AppUser> appUserRepository) : IAppUserValidation
{
    private readonly IRepository<Guid, AppUser> _appUserRepository = appUserRepository
        ?? throw new ArgumentNullException(nameof(appUserRepository));

    public async Task<AppUser> ValidateAdminOwnsUser(Guid userId, Guid adminUserId)
    {
        var appUser = await _appUserRepository.Query()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (appUser == null)
            throw new KeyNotFoundException("User not found");

        if (appUser.CreatedByUserId != adminUserId)
            throw new UnauthorizedAccessException("You are not allowed to manage this user.");

        return appUser;
    }
}
