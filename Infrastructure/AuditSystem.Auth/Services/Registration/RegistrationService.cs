using AuditSystem.Auth.Dtos;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

namespace AuditSystem.Auth.Services.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<RegistrationService> _logger;
        private readonly LinkGenerator _linkGenerator;
        private readonly IConfiguration _configuration;

        public RegistrationService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<RegistrationService> logger,
            IEmailService emailService,
            LinkGenerator linkGenerator,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailService = emailService;
            _linkGenerator = linkGenerator;
            _configuration = configuration;
        }

        public async Task RegisterAsync(RegisterDto request, string scheme, IUrlHelper urlHelper)
        {
            try
            {
                var role = await ValidateAndGetRoleAsync(request.RoleName);
                await ValidateUserDoesNotExistAsync(request.UserName);
                var newUser = await CreateUserAsync(request, role);
                var confirmationLink = await GenerateConfirmationLinkAsync(newUser, scheme, urlHelper);
                await SendConfirmationEmailAsync(newUser, confirmationLink);

                _logger.LogInformation("Confirmation email sent to {Email}", newUser.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for {Username}", request.UserName);
                throw;
            }
        }



        public async Task VerificationEmailAsync(VerificationEmailDto request, string scheme, IUrlHelper urlHelper)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new InvalidOperationException($"User with email '{request.Email}' does not exist");
                }

                var confirmationLink = await GenerateConfirmationLinkAsync(user, scheme, urlHelper);
                await SendConfirmationEmailAsync(user, confirmationLink);

                _logger.LogInformation("Confirmation email sent to {Email}", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Verfication Email failed for {UserEmail}", request.Email);
                throw;
            }
        }

        private async Task<Role> ValidateAndGetRoleAsync(string roleName)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
                throw new ArgumentException($"Role '{roleName}' does not exist");
            return role;
        }

        private async Task ValidateUserDoesNotExistAsync(string userName)
        {
            var existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser != null)
                throw new InvalidOperationException($"Username '{userName}' already exists");
        }

        private async Task<User> CreateUserAsync(RegisterDto request, Role role)
        {
            // 1. Create user object
            var newUser = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                RoleId = role.Id,
                CompanyId = request.CompanyId,
                DepartmentId = request.DepartmentId,
                SubDepartmentId = request.SubDepartmentId,
                IsActive = true,
                EmailConfirmed = false
            };

            // 2. Attempt creation
            var creationResult = await _userManager.CreateAsync(newUser, request.Password);

            // 3. Return if successful
            if (creationResult.Succeeded)
            {
                return newUser;
            }

            // 4. Handle failure
            await HandleFailedCreationAsync(request.UserName, creationResult);
            throw CreateExceptionFromErrors("User creation failed", creationResult.Errors);
        }

        private async Task HandleFailedCreationAsync(string username, IdentityResult creationResult)
        {
            var existingUser = await _userManager.FindByNameAsync(username);
            if (existingUser == null) return;

            var deletionResult = await _userManager.DeleteAsync(existingUser);
            if (!deletionResult.Succeeded)
            {
                throw CreateExceptionFromErrors(
                    "User creation failed and cleanup failed",
                    creationResult.Errors.Concat(deletionResult.Errors));
            }
        }

        private static InvalidOperationException CreateExceptionFromErrors(
            string prefix, IEnumerable<IdentityError> errors)
        {
            var errorDescriptions = errors.Select(e => e.Description);
            return new InvalidOperationException(
                $"{prefix}: {string.Join(", ", errorDescriptions)}");
        }

        private async Task<string> GenerateConfirmationLinkAsync(User user, string scheme, IUrlHelper urlHelper)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var domain = _configuration["AppSettings:Domain"];
            // Encode to prevent modification during transmission
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var base64Token = Convert.ToBase64String(tokenBytes);
            var encodedToken = WebUtility.UrlEncode(base64Token);

            var confirmationLink = $"{scheme}://{domain}/confirm-email?uid={user.Id}&token={encodedToken}";

            return confirmationLink;
        }
 
        private async Task SendConfirmationEmailAsync(User user, string confirmationLink)
        {
            await _emailService.SendEmailAsync(
                new EmailConfirmationRequest(
                    email: user.Email,
                    userName: user.UserName,
                    confirmationLink: confirmationLink
                )
            );
        }
    }
}