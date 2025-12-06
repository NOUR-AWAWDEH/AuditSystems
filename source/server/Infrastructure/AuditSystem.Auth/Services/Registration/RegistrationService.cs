using AuditSystem.Contract.Dtos.Register;
using AuditSystem.Auth.Services.Email;
using AuditSystem.DataAccess.Contexts;
using AuditSystem.Domain.Entities.Auth;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

namespace AuditSystem.Auth.Services.Registration;

public class RegistrationService(
    UserManager<ApplicationUser> userManager,
    RoleManager<Role> roleManager,
    ILogger<RegistrationService> logger,
    IEmailService emailService,
    IConfiguration configuration,
    ApplicationDbContext dbContext) : IRegistrationService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly IEmailService _emailService = emailService;
    private readonly ILogger<RegistrationService> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly ApplicationDbContext _dbContext = dbContext;

    // ========================= REGISTER =========================

    public async Task RegisterAsync(RegisterDto request, string scheme, Guid adminUserId)
    {
        // Check if a transaction is already in progress (e.g., from CommandTransactionBehavior)
        var currentTransaction = _dbContext.Database.CurrentTransaction;
        var localTransaction = currentTransaction == null
            ? await _dbContext.Database.BeginTransactionAsync()
            : null;

        try
        {
            var role = await ValidateAndGetRoleAsync(request.RoleName);
            await ValidateUserDoesNotExistAsync(request.UserName);

            // Get the admin's AppUser ID from their ApplicationUser ID (from JWT token)
            var adminAppUser = await _dbContext.AppUsers
                .FirstOrDefaultAsync(u => u.IdentityUserId == adminUserId) ??
                    throw new InvalidOperationException($"Admin user with Identity ID '{adminUserId}' not found");

            var identityUser = await CreateIdentityUserAsync(request);
            await AssignRoleAsync(identityUser, role.Name!);
            await CreateBusinessProfileAsync(identityUser, request, adminAppUser.Id);
            await CreateInitialRefreshTokenAsync(identityUser);

            var confirmationLink = await GenerateConfirmationLinkAsync(identityUser, scheme);
            await SendConfirmationEmailAsync(identityUser, confirmationLink);

            // Only commit if we started the transaction
            if (localTransaction != null)
            {
                await localTransaction.CommitAsync();
            }

            _logger.LogInformation("User registered and confirmation email sent: {Email}", identityUser.Email);
        }
        catch (Exception ex)
        {
            // Only rollback if we started the transaction
            if (localTransaction != null)
            {
                await localTransaction.RollbackAsync();
            }
            _logger.LogError(ex, "Registration failed for {Username}", request.UserName);
            throw;
        }
        finally
        {
            // Only dispose if we started the transaction
            if (localTransaction != null)
            {
                await localTransaction.DisposeAsync();
            }
        }
    }

    // ========================= VERIFY EMAIL =========================

    public async Task VerificationEmailAsync(VerificationEmailDto request, string scheme)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new InvalidOperationException($"User with email '{request.Email}' does not exist");

        var confirmationLink = await GenerateConfirmationLinkAsync(user, scheme);
        await SendConfirmationEmailAsync(user, confirmationLink);
    }

    // ========================= HELPERS =========================

    private async Task<Role> ValidateAndGetRoleAsync(string roleName) =>
        await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName)
            ?? throw new ArgumentException($"Role '{roleName}' does not exist");

    private async Task ValidateUserDoesNotExistAsync(string userName)
    {
        if (await _userManager.FindByNameAsync(userName) != null)
            throw new InvalidOperationException($"Username '{userName}' already exists");
    }

    private async Task<ApplicationUser> CreateIdentityUserAsync(RegisterDto request)
    {
        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw CreateExceptionFromErrors("Identity creation failed", result.Errors);

        return user;
    }

    private async Task AssignRoleAsync(ApplicationUser user, string roleName)
    {
        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
            throw CreateExceptionFromErrors("Role assignment failed", result.Errors);
    }

    private async Task CreateBusinessProfileAsync(ApplicationUser identityUser, RegisterDto request, Guid adminAppUserId)
    {
        var appUser = new AppUser
        {
            Id = identityUser.Id, // Shared Primary Key with IdentityUser
            IdentityUserId = identityUser.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = identityUser.UserName,
            Role = request.RoleName,
            CompanyId = request.CompanyId,
            JobTitleId = request.JobTitleId,
            SubDepartmentId = request.SubDepartmentId,
            IsActive = true,
            CreatedByUserId = adminAppUserId // Admin who created this user
        };

        _dbContext.AppUsers.Add(appUser);
        await _dbContext.SaveChangesAsync();
    }


    private async Task CreateInitialRefreshTokenAsync(ApplicationUser user)
    {
        _dbContext.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            UserId = user.Id
        });

        await _dbContext.SaveChangesAsync();
    }

    private async Task<string> GenerateConfirmationLinkAsync(ApplicationUser user, string scheme)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var domain = _configuration["APP_DOMAIN"]
            ?? throw new InvalidOperationException("APP_DOMAIN is not configured.");

        var encodedToken = WebUtility.UrlEncode(
            Convert.ToBase64String(Encoding.UTF8.GetBytes(token)));

        return $"{scheme}://{domain}/confirm-email?uid={user.Id}&token={encodedToken}";
    }

    private async Task SendConfirmationEmailAsync(ApplicationUser user, string confirmationLink)
    {
        if (user.Email == null || user.UserName == null)
            throw new InvalidOperationException("User email or username is null.");

        await _emailService.SendEmailAsync(new EmailConfirmationRequest(
            user.Email,
            user.UserName,
            confirmationLink
        ));
    }

    private static InvalidOperationException CreateExceptionFromErrors(string prefix, IEnumerable<IdentityError> errors)
        => new($"{prefix}: {string.Join(", ", errors.Select(e => e.Description))}");
}
