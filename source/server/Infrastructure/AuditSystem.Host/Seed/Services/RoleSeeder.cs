using AuditSystem.Domain.Entities.Users;
using AuditSystem.Host.Seed.Exceptions;
using AuditSystem.Host.Seed.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Host.Seed.Services;

public sealed class RoleSeeder : IRoleSeeder
{
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<RoleSeeder> _logger;

    public RoleSeeder(RoleManager<Role> roleManager, ILogger<RoleSeeder> logger)
    {
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SeedRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = new[] { Role.Admin, Role.ActionOwner };
        
        foreach (var roleName in roles)
        {
            await SeedRoleAsync(roleName, cancellationToken);
        }
    }

    private async Task SeedRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
        {
            _logger.LogDebug("Role {RoleName} already exists", roleName);
            return;
        }

        var role = new Role { Name = roleName, NormalizedName = roleName.ToUpperInvariant() };
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to create role {RoleName}: {Errors}", roleName, errors);
            throw new SeedingException($"Role creation failed for {roleName}");
        }

        _logger.LogInformation("Role {RoleName} created successfully", roleName);
    }
}