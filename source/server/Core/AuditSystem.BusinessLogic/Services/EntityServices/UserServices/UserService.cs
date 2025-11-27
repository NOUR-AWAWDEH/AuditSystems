using System.Linq;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.UserServices;

internal sealed class UserService(
    UserManager<AppUser> userManager,
    ILogger<UserService> logger) : IUserService
{
    public async Task<List<UserModel>> GetActionOwnersByUserIdAsync(Guid userId)
    {
        logger.LogDebug("Fetching ActionOwners for User {UserId}", userId);

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            logger.LogWarning("User with ID {UserId} not found", userId);
            return new List<UserModel>();
        }

        var actionOwners = await userManager.GetUsersInRoleAsync(Role.ActionOwner);

        var companyActionOwners = actionOwners
            .Where(u => u.CreatedByUserId == user.Id)
            .Select(u => new UserModel(
                u.Id,
                u.FirstName,
                u.LastName,
                u.IdentityUser.Email ?? string.Empty,
                Role.ActionOwner,
                u.SubDepartment ?? null!,
                u.JobTitle))
            .ToList();

        logger.LogDebug("Retrieved {Count} ActionOwners for Company {CompanyId}", companyActionOwners.Count, user.CompanyId);
        return companyActionOwners;
    }
}
