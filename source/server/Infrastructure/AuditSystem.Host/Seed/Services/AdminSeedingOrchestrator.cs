using AuditSystem.Domain.Entities.Users;
using AuditSystem.DataAccess.Contexts;
using Microsoft.AspNetCore.Identity;
using AuditSystem.Host.Seed.Interfaces;

namespace AuditSystem.Host.Seed.Services;

public class AdminSeedingOrchestrator(
    UserManager<ApplicationUser> userManager,
    RoleManager<Role> roleManager,
    ApplicationDbContext dbContext,
    ILogger<AdminSeedingOrchestrator> logger
) : IAdminSeedingOrchestrator
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly ILogger<AdminSeedingOrchestrator> _logger = logger;

    public async Task SeedAsync()
    {
        _logger.LogInformation("Starting admin user seeding...");

        var config = LoadAdminConfiguration();
        await ValidateCompanyExistsAsync(config.CompanyId);
        await EnsureRequiredRolesAsync();
        await CreateAdminUserIfMissingAsync(config);

        _logger.LogInformation("Admin seeding completed.");
    }

    private sealed record AdminConfig(
        string Email,
        string Password,
        string Username,
        Guid CompanyId
    );

    // -----------------------------
    // CONFIG LOADING
    // -----------------------------
    private static AdminConfig LoadAdminConfiguration()
    {
        return new AdminConfig(
            GetRequiredEnvironmentVariable("ADMIN_INITIAL_EMAIL"),
            GetRequiredEnvironmentVariable("ADMIN_INITIAL_PASSWORD"),
            GetRequiredEnvironmentVariable("ADMIN_INITIAL_USERNAME"),
            GetRequiredGuidEnvironmentVariable("ADMIN_INITIAL_COMPANY_ID")
        );
    }

    private static string GetRequiredEnvironmentVariable(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);

        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"{key} is not set or empty.");

        return value.Trim();
    }

    private static Guid GetRequiredGuidEnvironmentVariable(string key)
    {
        var value = GetRequiredEnvironmentVariable(key);

        if (!Guid.TryParse(value, out var guid))
            throw new InvalidOperationException($"{key} must be a valid GUID. Current value: {value}");

        return guid;
    }

    // -----------------------------
    // COMPANY VALIDATION
    // -----------------------------
    private async Task ValidateCompanyExistsAsync(Guid companyId)
    {
        var company = await _dbContext.Companies.FindAsync(companyId);

        if (company is null)
        {
            _logger.LogError("Company {CompanyId} not found in DB.", companyId);
            throw new InvalidOperationException($"Company not found: {companyId}. Run migrations first.");
        }

        _logger.LogInformation("Company validated: {CompanyName}", company.Name);
    }

    // -----------------------------
    // ROLE CREATION
    // -----------------------------
    private async Task EnsureRequiredRolesAsync()
    {
        string[] roles = { Role.Admin, Role.ActionOwner };

        foreach (var roleName in roles)
            await EnsureRoleExistsAsync(roleName);
    }

    private async Task EnsureRoleExistsAsync(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
            return;

        var role = new Role
        {
            Name = roleName,
            NormalizedName = roleName.ToUpper()
        };

        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            _logger.LogError("Role creation failed for {Role}: {Errors}", roleName, errors);
            throw new InvalidOperationException($"Cannot create role {roleName}: {errors}");
        }

        _logger.LogInformation("Role {Role} created.", roleName);
    }

    // -----------------------------
    // ADMIN USER SEEDING
    // -----------------------------
    private async Task CreateAdminUserIfMissingAsync(AdminConfig config)
    {
        var existing = await _userManager.FindByEmailAsync(config.Email);

        if (existing is not null)
        {
            _logger.LogInformation("Admin user already exists: {Email}", config.Email);

            // Ensure role still assigned
            if (!await _userManager.IsInRoleAsync(existing, Role.Admin))
                await _userManager.AddToRoleAsync(existing, Role.Admin);

            return;
        }

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = config.Username,
            Email = config.Email,
            EmailConfirmed = true,
            IsActive = true,
            FirstName = "System",
            LastName = "Admin",
            CompanyId = config.CompanyId
        };

        var createResult = await _userManager.CreateAsync(user, config.Password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(x => x.Description));
            _logger.LogError("Admin creation failed: {Errors}", errors);
            throw new InvalidOperationException($"Admin user creation failed: {errors}");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, Role.Admin);

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(x => x.Description));
            _logger.LogError("Role assignment failed: {Errors}", errors);
            throw new InvalidOperationException($"Admin role assignment failed: {errors}");
        }

        _logger.LogInformation("Admin user created and role assigned: {Email}", config.Email);
    }
}
