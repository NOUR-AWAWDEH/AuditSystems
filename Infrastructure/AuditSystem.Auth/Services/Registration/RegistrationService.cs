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

namespace AuditSystem.Auth.Services.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<RegistrationService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;
        private readonly IConfiguration _configuration;

        public RegistrationService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<RegistrationService> logger,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator,
            IConfiguration configuration)

        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
            _configuration = configuration;
        }

        public async Task RegisterAsync(RegisterDto request, string scheme, IUrlHelper urlHelper)
        {
            try
            {
                var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == request.RoleName);
                if (role == null)
                    throw new ArgumentException($"Role '{request.RoleName}' does not exist");

                var existingUser = await _userManager.FindByNameAsync(request.UserName);
                if (existingUser != null)
                    throw new InvalidOperationException($"Username '{request.UserName}' already exists");

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
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(newUser, request.Password);
                if (!result.Succeeded)
                    throw new InvalidOperationException($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                if (!string.IsNullOrEmpty(token))
                {

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for {Username}", request.UserName);
                throw;
            }
        }
    }
}
