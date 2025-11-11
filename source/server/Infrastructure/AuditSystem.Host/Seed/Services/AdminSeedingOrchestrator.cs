using AuditSystem.Domain.Entities.Users;
using AuditSystem.DataAccess.Contexts;
using Microsoft.AspNetCore.Identity;
using AuditSystem.Host.Seed.Interfaces;

namespace AuditSystem.Host.Seed.Services;

public class AdminSeedingOrchestrator : IAdminSeedingOrchestrator
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AdminSeedingOrchestrator> _logger;

    public AdminSeedingOrchestrator(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ApplicationDbContext dbContext,
        ILogger<AdminSeedingOrchestrator> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Starting admin user seeding via orchestrator...");
        
        try
        {
            var config = LoadAdminConfiguration();
            await SeedAdminUserAsync(config);
            
            _logger.LogInformation("Admin user seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Admin seeding failed");
            throw;
        }
    }

    private record AdminConfig(
        string Email,
        string Password,
        string Username,
        Guid CompanyId
    );

    private static AdminConfig LoadAdminConfiguration()
    {
        var email = GetRequiredEnvironmentVariable("ADMIN_INITIAL_EMAIL");
        var password = GetRequiredEnvironmentVariable("ADMIN_INITIAL_PASSWORD");
        var username = GetRequiredEnvironmentVariable("ADMIN_INITIAL_USERNAME");
        var companyId = GetRequiredGuidEnvironmentVariable("ADMIN_INITIAL_COMPANY_ID");

        return new AdminConfig(email, password, username, companyId);
    }

    private static string GetRequiredEnvironmentVariable(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName);
        return string.IsNullOrWhiteSpace(value) 
            ? throw new InvalidOperationException($"{variableName} is not set or empty")
            : value.Trim();
    }

    private static Guid GetRequiredGuidEnvironmentVariable(string variableName)
    {
        var value = GetRequiredEnvironmentVariable(variableName);
        return Guid.TryParse(value, out var guid) 
            ? guid 
            : throw new InvalidOperationException($"{variableName} is not a valid GUID: {value}");
    }

    private async Task SeedAdminUserAsync(AdminConfig config)
    {
        await ValidateCompanyExistsAsync(config.CompanyId);
        await EnsureRolesAsync();
        
        var adminUser = await CreateAdminUserIfNotExistsAsync(config);
        
        if (adminUser is not null)
        {
            // Role is now assigned during user creation, no need for separate assignment
            // await AssignAdminRoleAsync(adminUser);
        }
    }

    private async Task ValidateCompanyExistsAsync(Guid companyId)
    {
        var company = await _dbContext.Companies.FindAsync(companyId);
        if (company is null)
        {
            _logger.LogError("Company {CompanyId} not found in database", companyId);
            throw new InvalidOperationException($"Company with ID {companyId} not found. Run migrations first.");
        }

        _logger.LogInformation("Company validation passed: {CompanyName}", company.Name);
    }

    private async Task EnsureRolesAsync()
    {
        var roles = new[] { Role.Admin, Role.ActionOwner };

        foreach (var roleName in roles)
        {
            await EnsureRoleAsync(roleName);
        }
    }

    private async Task EnsureRoleAsync(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
        {
            _logger.LogDebug("Role {RoleName} already exists", roleName);
            return;
        }

        var role = new Role { Name = roleName, NormalizedName = roleName.ToUpper() };
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to create role {RoleName}: {Errors}", roleName, errors);
            throw new InvalidOperationException($"Role creation failed for {roleName}");
        }

        _logger.LogInformation("Role {RoleName} created successfully", roleName);
    }

    private async Task<User?> CreateAdminUserIfNotExistsAsync(AdminConfig config)
    {
        var existingUser = await _userManager.FindByEmailAsync(config.Email);
        if (existingUser is not null)
        {
            _logger.LogInformation("Admin user already exists: {Email}", config.Email);
            return null;
        }

        // Retrieve the Admin role
        var adminRole = await _roleManager.FindByNameAsync(Role.Admin);
        if (adminRole == null)
        {
            _logger.LogError("Admin role not found during user creation.");
            throw new InvalidOperationException("Admin role not found. Ensure roles are seeded correctly.");
        }

        var adminUser = new User
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

        var result = await _userManager.CreateAsync(adminUser, config.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Admin user creation failed: {Errors}", errors);
            throw new InvalidOperationException($"User creation failed: {errors}");
        }

        // Assign the Admin role to the newly created user
        var roleAssignmentResult = await _userManager.AddToRoleAsync(adminUser, Role.Admin);
        if (!roleAssignmentResult.Succeeded)
        {
            var errors = string.Join(", ", roleAssignmentResult.Errors.Select(e => e.Description));
            _logger.LogError("Failed to assign Admin role to user {Email}: {Errors}", config.Email, errors);
            throw new InvalidOperationException($"Role assignment failed for user {config.Email}: {errors}");
        }

        _logger.LogInformation("Admin user created successfully and Admin role assigned: {Email}", config.Email);
        return adminUser;
    }
}