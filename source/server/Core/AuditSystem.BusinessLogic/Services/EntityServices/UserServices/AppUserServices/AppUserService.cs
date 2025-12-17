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

    public async Task CreateUserAsync(RegisterDto request, Guid adminUserId)
    {
        await appUserValidation.ValidateRoleNotAdmin(request.RoleName);

        appLogger.LogInformation("Registering user {UserName} starting...", request.UserName);
        await registrationService.RegisterAsync(request, adminUserId);
        appLogger.LogInformation("user has been registered successfully");
    }

    public async Task DeleteUserAsync(Guid userId, Guid adminUserId)
    {
        appLogger.LogInformation("Deleting user {UserId} by admin {AdminId}", userId, adminUserId);

        //  1. Validate admin owns the user  and bring the user 
        var appUser = await appUserValidation.ValidateAdminOwnsUser(userId, adminUserId);

        //  2. Validate identity user exists
        var identityUser = await appUserValidation.ValidateIdentityUserExists(appUser.IdentityUserId);

        //  4. Delete AppUser first (Dependent)
        await appUserRepository.DeleteAsync(appUser.Id);

        //  5. Delete identity user (Principal)
        var identityResult = await userManager.DeleteAsync(identityUser);
        if (!identityResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", identityResult.Errors.Select(e => e.Description)));
        }

        appLogger.LogInformation("User {UserId} deleted successfully", userId);
    }

    public async Task UpdateUserAsync(UserModel user, Guid adminUserId)
    {
        appLogger.LogInformation("Updating user {UserId} by admin {AdminId}", user.Id, adminUserId);

        // 1. Validate admin owns the user
        var appUser = await appUserValidation.ValidateAdminOwnsUser(user.Id, adminUserId);

        // 2. Validate identity user exists
        var identityUser = await appUserValidation.ValidateIdentityUserExists(appUser.IdentityUserId);

        // 3. Validate role is not admin 
        await appUserValidation.ValidateRoleNotAdmin(user.Role);

        // 4. Update AppUser properties
        appUser.FirstName = user.FirstName;
        appUser.LastName = user.LastName;
        appUser.Email = user.Email;
        appUser.UserName = user.UserName;
        appUser.Role = user.Role;
        appUser.SubDepartmentId = user.SubDepartment?.Id;
        appUser.JobTitleId = user.JobTitle?.Id ?? appUser.JobTitleId;
        appUser.UpdatedAt = DateTime.UtcNow;

        // 5. Update ApplicationUser (Identity) properties
        identityUser.Email = user.Email;
        identityUser.NormalizedEmail = user.Email.ToUpperInvariant();
        identityUser.UserName = user.UserName;
        identityUser.NormalizedUserName = user.UserName.ToUpperInvariant();

        // 6. Save ApplicationUser changes
        var identityResult = await userManager.UpdateAsync(identityUser);
        if (!identityResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", identityResult.Errors.Select(e => e.Description)));
        }

        // 7. Save AppUser changes
        await appUserRepository.UpdateAsync(appUser);

        appLogger.LogInformation("User {UserId} updated successfully", user.Id);
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
                u.IdentityUser.UserName ?? string.Empty,
                u.FirstName,
                u.LastName,
                u.IdentityUser.Email ?? string.Empty,
                u.Role,
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

    public async Task<UserModel> GetUserByIdAsync(Guid id, Guid adminUserId)
    {
        appLogger.LogDebug("Retrieving user {UserId} for admin {AdminId}", id, adminUserId);

        // 1. Get user and validate ownership
        var appUser = await appUserValidation.ValidateAdminOwnsUser(id, adminUserId);

        // 2. Load related entities
        var userWithRelations = await appUserRepository.Query()
            .Include(u => u.IdentityUser)
            .Include(u => u.SubDepartment)
            .Include(u => u.JobTitle)
            .FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new KeyNotFoundException("User not found");

        // 3. Map to UserModel
        var userModel = new UserModel(
            userWithRelations.Id,
            userWithRelations.IdentityUser.UserName ?? string.Empty,
            userWithRelations.FirstName,
            userWithRelations.LastName,
            userWithRelations.IdentityUser.Email ?? string.Empty,
            userWithRelations.Role,
            userWithRelations.SubDepartment == null ? null : new SubDepartmentModel
            {
                Id = userWithRelations.SubDepartment.Id,
                Name = userWithRelations.SubDepartment.Name,
                DepartmentId = userWithRelations.SubDepartment.DepartmentId
            },
            userWithRelations.JobTitle == null ? null : new JobTitleModel
            {
                Id = userWithRelations.JobTitle.Id,
                Name = userWithRelations.JobTitle.Name,
                CompanyId = userWithRelations.JobTitle.CompanyId
            });

        appLogger.LogDebug("Retrieved user {UserId} successfully", id);
        return userModel;
    }

    public async Task<IReadOnlyList<UserModel>> GetUsersByAdminIdAsync(Guid adminUserId)
    {
        appLogger.LogDebug("Retrieving all users for admin {AdminId}", adminUserId);

        // 1. Validate admin exists
        var adminAppUser = await appUserValidation.ValidateAppUserExists(adminUserId);

        // 2. Query all users created by this admin
        var users = await appUserRepository.Query()
            .Where(u => u.CreatedByUserId == adminAppUser.Id)
            .Include(u => u.IdentityUser)
            .Include(u => u.SubDepartment)
            .Include(u => u.JobTitle)
            .AsNoTracking()
            .ToListAsync();

        // 3. Map to UserModel list with roles
        var userModels = new List<UserModel>();
        foreach (var user in users)
        {
            var userModel = new UserModel(
                user.Id,
                user.IdentityUser.UserName ?? string.Empty,
                user.FirstName,
                user.LastName,
                user.IdentityUser.Email ?? string.Empty,
                user.Role,
                user.SubDepartment == null ? null : new SubDepartmentModel
                {
                    Id = user.SubDepartment.Id,
                    Name = user.SubDepartment.Name,
                    DepartmentId = user.SubDepartment.DepartmentId
                },
                user.JobTitle == null ? null : new JobTitleModel
                {
                    Id = user.JobTitle.Id,
                    Name = user.JobTitle.Name,
                    CompanyId = user.JobTitle.CompanyId
                });

            userModels.Add(userModel);
        }

        appLogger.LogInformation("Retrieved {Count} users for admin {AdminId}", userModels.Count, adminUserId);
        return userModels.AsReadOnly();
    }

    public async Task<PaginationResult<UserModel>> SearchByAdminIdAsync(Guid adminUserId, string? searchTerm, Page page)
    {
        appLogger.LogDebug("Searching users for admin {AdminId} with term: {SearchTerm}",
            adminUserId, searchTerm ?? "(none)");

        // 1. Validate admin exists
        var adminAppUser = await appUserValidation.ValidateAppUserExists(adminUserId);

        // 2. Use the search helper to get paginated results
        var (cacheKey, countCacheKey) = appUserSearch.GetUserSearchCacheKeys(
            adminAppUser.Id, searchTerm, null, page);

        var result = await appUserSearch.GetUserSearchFromRepositoryAsync(
            adminAppUser.Id, searchTerm, null, page, cacheKey, countCacheKey);

        appLogger.LogInformation("Search returned {Count} users for admin {AdminId}",
            result.TotalCount, adminUserId);

        return result;
    }
}