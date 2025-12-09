using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.BusinessLogic.Helpers.UserHelpers.AppUserHelpers;

public class AppUserValidationHelper(
    IRepository<Guid, AppUser> appUserRepository,
    UserManager<ApplicationUser> userManager) : IAppUserValidation
{
    private readonly IRepository<Guid, AppUser> _appUserRepository =
        appUserRepository ?? throw new ArgumentNullException(nameof(appUserRepository));

    private readonly UserManager<ApplicationUser> _userManager =
        userManager ?? throw new ArgumentNullException(nameof(userManager));

    public async Task<AppUser> ValidateAdminOwnsUser(Guid userId, Guid adminUserId)
    {
        var appUser = await _appUserRepository.GetByIdAsync(userId)
            ?? throw new KeyNotFoundException("User not found");

        if (appUser.CreatedByUserId != adminUserId)
            throw new UnauthorizedAccessException("You are not allowed to manage this user.");

        return appUser;
    }

    public async Task<ApplicationUser> ValidateIdentityUserExists(Guid userId)
    {
        var identityUser = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new InvalidOperationException("Identity user not found");

        return identityUser;
    }

    public async Task<AppUser> ValidateAppUserExists(Guid userId)
    {
        var appUser = await _appUserRepository.GetByIdAsync(userId)
            ?? throw new KeyNotFoundException("User not found");

        return appUser;
    }

    public Task ValidateRoleNotAdmin(string roleName)
    {
        if (roleName == Role.Admin)
            throw new InvalidOperationException(
                "🐔🚫 Admin creation detected! Nice try 😎, Admin spawning 🐥 is forbidden in this universe 🚫 🪐"
            );

        return Task.CompletedTask;
    }
}
