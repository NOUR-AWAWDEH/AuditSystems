using System.Linq;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.UserServices;

internal sealed class UserService(
    UserManager<ApplicationUser> userManager,
    IRepository<Guid, AppUser> appUserRepository,
    ILogger<UserService> logger) : IUserService
{
    public async Task<List<UserModel>> GetActionOwnersByUserIdAsync(Guid userId)
    {
        logger.LogDebug("Fetching ActionOwners for User {UserId}", userId);

        // 1. Find the current AppUser corresponding to the Identity User ID
        var currentAppUser = await appUserRepository.Query()
            .FirstOrDefaultAsync(u => u.IdentityUserId == userId);

        if (currentAppUser == null)
        {
            logger.LogWarning("AppUser with IdentityUserId {UserId} not found", userId);
            return new List<UserModel>();
        }

        // 2. Get all Identity Users in the "ActionOwner" role
        var actionOwnerIdentityUsers = await userManager.GetUsersInRoleAsync(Role.ActionOwner);
        var actionOwnerIdentityIds = actionOwnerIdentityUsers.Select(u => u.Id).ToList();

        if (!actionOwnerIdentityIds.Any())
        {
            return new List<UserModel>();
        }

        // 3. Query AppUsers that are in the role AND created by the current user
        var actionOwners = await appUserRepository.Query()
            .Include(u => u.IdentityUser)
            .Include(u => u.SubDepartment)
            .Include(u => u.JobTitle)
            .Where(u => actionOwnerIdentityIds.Contains(u.IdentityUserId))
            .Where(u => u.CreatedByUserId == currentAppUser.Id)
            .ToListAsync();

        var userModels = actionOwners
            .Select(u => new UserModel(
                u.Id,
                u.FirstName,
                u.LastName,
                u.IdentityUser.Email ?? string.Empty,
                Role.ActionOwner,
                u.SubDepartment ?? null!,
                u.JobTitle))
            .ToList();

        logger.LogDebug("Retrieved {Count} ActionOwners created by {CreatorId}", userModels.Count, currentAppUser.Id);
        return userModels;
    }
}