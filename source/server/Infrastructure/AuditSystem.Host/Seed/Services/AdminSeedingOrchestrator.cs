using AuditSystem.Domain.Entities.Users;
using AuditSystem.DataAccess.Contexts;
using Microsoft.AspNetCore.Identity;
using AuditSystem.Host.Seed.Interfaces;
using Microsoft.EntityFrameworkCore;
using AuditSystem.Domain.Entities.Organization;

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
        await ValidateJobTitleExistsAsync(config.JobTitleId);
        await EnsureRequiredRolesAsync();
        await CreateAdminUserIfMissingAsync(config);

        _logger.LogInformation("Admin seeding completed.");
    }

    private sealed record AdminConfig(
        string Email,
        string Password,
        string Username,
        string FirstName,
        string LastName,
        Guid CompanyId,
        Guid JobTitleId
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
            GetOptionalEnvironmentVariable("ADMIN_INITIAL_FIRSTNAME", "System"),
            GetOptionalEnvironmentVariable("ADMIN_INITIAL_LASTNAME", "Admin"),
            GetRequiredGuidEnvironmentVariable("ADMIN_INITIAL_COMPANY_ID"),
            GetRequiredGuidEnvironmentVariable("ADMIN_INITIAL_JOBTITLE_ID")
        );
    }

    private static string GetRequiredEnvironmentVariable(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);

        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"{key} is not set or empty.");

        return value.Trim();
    }

    private static string GetOptionalEnvironmentVariable(string key, string defaultValue)
    {
        var value = Environment.GetEnvironmentVariable(key);
        return string.IsNullOrWhiteSpace(value) ? defaultValue : value.Trim();
    }

    private static Guid GetRequiredGuidEnvironmentVariable(string key)
    {
        var value = GetRequiredEnvironmentVariable(key);

        if (!Guid.TryParse(value, out var guid))
            throw new InvalidOperationException($"{key} must be a valid GUID. Current value: {value}");

        return guid;
    }

    // -----------------------------
    // VALIDATION
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

    private async Task ValidateJobTitleExistsAsync(Guid jobTitleId)
    {
        var jobTitle = await _dbContext.Set<JobTitle>().FindAsync(jobTitleId);

        if (jobTitle is null)
        {
            _logger.LogError("JobTitle {JobTitleId} not found in DB.", jobTitleId);
            throw new InvalidOperationException($"JobTitle not found: {jobTitleId}. Ensure JobTitle is seeded first.");
        }

        _logger.LogInformation("JobTitle validated: {JobTitleName}", jobTitle.Name);
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
        // Check if ApplicationUser already exists
        var existingIdentityUser = await _userManager.FindByEmailAsync(config.Email);

        if (existingIdentityUser is not null)
        {
            _logger.LogInformation("Admin ApplicationUser already exists: {Email}", config.Email);

            // Ensure role still assigned
            if (!await _userManager.IsInRoleAsync(existingIdentityUser, Role.Admin))
                await _userManager.AddToRoleAsync(existingIdentityUser, Role.Admin);

            // Check if AppUser exists
            var existingAppUser = await _dbContext.AppUsers
                .FirstOrDefaultAsync(au => au.IdentityUserId == existingIdentityUser.Id);

            if (existingAppUser is null)
            {
                _logger.LogWarning("ApplicationUser exists but AppUser is missing. Creating AppUser for {Email}", config.Email);
                await CreateAppUserAsync(existingIdentityUser.Id, config);
            }

            return;
        }

        // Create new ApplicationUser (Identity)
        var identityUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = config.Username,
            Email = config.Email,
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(identityUser, config.Password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(x => x.Description));
            _logger.LogError("ApplicationUser creation failed: {Errors}", errors);
            throw new InvalidOperationException($"ApplicationUser creation failed: {errors}");
        }

        _logger.LogInformation("ApplicationUser created: {Email}", config.Email);

        // Assign Admin role
        var roleResult = await _userManager.AddToRoleAsync(identityUser, Role.Admin);

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(x => x.Description));
            _logger.LogError("Role assignment failed: {Errors}", errors);
            throw new InvalidOperationException($"Admin role assignment failed: {errors}");
        }

        _logger.LogInformation("Admin role assigned to: {Email}", config.Email);

        // Create linked AppUser (Business entity)
        await CreateAppUserAsync(identityUser.Id, config);

        _logger.LogInformation("Admin user fully created: {Email}", config.Email);
    }

    private async Task CreateAppUserAsync(Guid identityUserId, AdminConfig config)
    {

        var appUser = new AppUser
        {
            Id = identityUserId,
            IdentityUserId = identityUserId,
            FirstName = config.FirstName,
            LastName = config.LastName,
            Email = config.Email,
            UserName = config.Username,
            CompanyId = config.CompanyId,
            JobTitleId = config.JobTitleId,
            IsActive = true,
            SubDepartmentId = null,
            // Self-reference for the first admin user
            CreatedByUserId = identityUserId
        };

        _dbContext.AppUsers.Add(appUser);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("AppUser created and linked to ApplicationUser: {Email}", config.Email);
    }
}
