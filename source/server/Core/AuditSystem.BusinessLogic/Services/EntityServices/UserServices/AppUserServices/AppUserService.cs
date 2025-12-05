using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AuditSystem.Auth.Services.Registration;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using AuditSystem.Contract.Dtos.Register;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.BusinessLogic.Services.EntityServices.UserServices.AppUserServices;

internal sealed class AppUserService(
    IRepository<Guid, AppUser> appUserRepository,
    UserManager<ApplicationUser> userManager,
    IAppUserSearch appUserSearch,
    IAppUserValidation appUserValidation,
    IRegistrationService registrationService,
    ILogger<AppUserService> appLogger) :
    IAppUserService
{
    private static readonly List<UserModel> EmptyUserList = [];

    public IAppUserSearch AppUserSearch => appUserSearch;
    public IAppUserValidation AppUserValidation => appUserValidation;
    public IRegistrationService RegistrationService => registrationService;
    public UserManager<ApplicationUser> UserManager => userManager;

    public async Task CreateUserAsync(RegisterDto request, Guid adminUserId, string scheme)
    {
        appLogger.LogInformation("Registering user {UserName} starting...", request.UserName);
        await registrationService.RegisterAsync(request, scheme, adminUserId);
        appLogger.LogInformation("user has been registered successfully");
    }

    public async Task DeleteUserAsync(Guid userId, Guid adminUserId)
    {
        appLogger.LogInformation("Deleting user {UserId} by admin {AdminId}", userId, adminUserId);

        //  1. Validate admin owns the user  and bring the user 
        var appUser = await appUserValidation.ValidateAdminOwnsUser(userId, adminUserId);

        //  2. Validate identity user exists
        var identityUser = await appUserValidation.ValidateIdentityUserExists(appUser.IdentityUserId);

        //  4. Begin transaction ONLY after all validation passes
        await using var transaction = await appUserRepository.BeginTransactionAsync();

        try
        {
            //  4. Delete identity user first
            var identityResult = await userManager.DeleteAsync(identityUser);
            if (!identityResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            }

            //  5. Delete AppUser
            await appUserRepository.DeleteAsync(appUser.Id);

            //  6. Commit transaction
            await transaction.CommitAsync();

            appLogger.LogInformation("User {UserId} deleted successfully", userId);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public Task UpdateUserAsync(UserModel user, Guid adminUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<UserModel>> GetActionOwnersByAdminIdAsync(Guid adminUserId)
    {
        appLogger.LogDebug("Fetching ActionOwners for Admin {AdminId}", adminUserId);

        // 1. Find the current admin at the appUserRepository 
        var adminAppUser = await appUserValidation.ValidateAppUserExists(adminUserId);

        if (adminAppUser == null)
        {
            appLogger.LogWarning("AppUser with IdentityUserId {UserId} not found", adminUserId);
            return EmptyUserList;
        }

        // 2. Get all Identity Users in the "ActionOwner" role
        var actionOwnerIdentityUsers = await userManager.GetUsersInRoleAsync(Role.ActionOwner);
        var actionOwnerIdentityIds = actionOwnerIdentityUsers.Select(u => u.Id).ToList();

        if (actionOwnerIdentityIds.Count == 0)
        {
            return EmptyUserList;
        }

        // 3. Query AppUsers that are in the role AND created by the current user
        var userModels = await appUserRepository.Query()
            .Where(u => actionOwnerIdentityIds.Contains(u.IdentityUserId))
            .Where(u => u.CreatedByUserId == adminAppUser.Id)
            .Select(u => new UserModel(
                u.Id,
                u.FirstName,
                u.LastName,
                u.IdentityUser.Email ?? string.Empty,
                Role.ActionOwner,
                u.SubDepartment == null ? null : new SubDepartmentModel
                {
                    Id = u.SubDepartment.Id,
                    Name = u.SubDepartment.Name,
                    DepartmentId = u.SubDepartment.DepartmentId
                },
                u.JobTitle == null ? null : new JobTitleModel
                {
                    Id = u.JobTitle.Id,
                    Name = u.JobTitle.Name,
                    CompanyId = u.JobTitle.CompanyId
                }))
            .ToListAsync();

        appLogger.LogInformation("Retrieved {Count} ActionOwners created by {CreatorId}", userModels.Count, adminAppUser.Id);
        return userModels;
    }

    public Task<UserModel> GetUserByIdAsync(Guid id, Guid adminUserId)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<UserModel>> GetUsersByAdminIdAsync(Guid adminUserId)
    {
        throw new NotImplementedException();
    }

    public Task<PaginationResult<UserModel>> SearchByAdminIdAsync(Guid adminUserId, string? searchTerm, Page page)
    {
        throw new NotImplementedException();
    }
}