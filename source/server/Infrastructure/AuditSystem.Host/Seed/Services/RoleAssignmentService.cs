using AuditSystem.Domain.Entities.Users;
using AuditSystem.Host.Seed.Exceptions;
using AuditSystem.Host.Seed.Interfaces;
using Microsoft.AspNetCore.Identity;

public sealed class RoleAssignmentService : IRoleAssignmentService
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RoleAssignmentService> _logger;

    public RoleAssignmentService(UserManager<User> userManager, ILogger<RoleAssignmentService> logger)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AssignAdminRoleAsync(User user)
    {
        if (await _userManager.IsInRoleAsync(user, Role.Admin))
        {
            _logger.LogDebug("User {Email} already has Admin role", user.Email);
            return;
        }

        var result = await _userManager.AddToRoleAsync(user, Role.Admin);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to assign Admin role: {Errors}", errors);
            throw new SeedingException($"Role assignment failed: {errors}");
        }

        _logger.LogInformation("Admin role assigned to user: {Email}", user.Email);
    }
}