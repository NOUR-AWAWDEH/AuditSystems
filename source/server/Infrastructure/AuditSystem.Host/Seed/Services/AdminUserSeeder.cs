namespace AuditSystem.Host.Seed.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using AuditSystem.Domain.Entities.Users;
using AuditSystem.Host.Seed.Interfaces;
using AuditSystem.Host.Seed.Exceptions;

public sealed class AdminUserSeeder : IAdminUserSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AdminUserSeeder> _logger;

    public AdminUserSeeder(UserManager<User> userManager, ILogger<AdminUserSeeder> logger)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<User?> SeedAdminUserAsync(string email, string username, string password, Guid companyId)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            _logger.LogInformation("Admin user already exists: {Email}", email);
            return null;
        }

        var adminUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = username,
            Email = email,
            EmailConfirmed = true,
            IsActive = true,
            FirstName = "System",
            LastName = "Admin",
            CompanyId = companyId
        };

        var result = await _userManager.CreateAsync(adminUser, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Admin user creation failed: {Errors}", errors);
            throw new SeedingException($"User creation failed: {errors}");
        }

        _logger.LogInformation("Admin user created successfully: {Email}", email);
        return adminUser;
    }
}