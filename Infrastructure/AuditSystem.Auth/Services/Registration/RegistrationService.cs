using AuditSystem.Auth.Dtos;
using AuditSystem.Auth.Models;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Services.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegistrationService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult?> RegisterAsync(RegisterDto request)
        {
            string roleName = Enum.GetName(typeof(UserRole), request.RoleName)
                ?? throw new ArgumentException("Invalid role.");

            if (await _userManager.FindByNameAsync(request.UserName) != null)
                return IdentityResult.Failed(new IdentityError { Description = "Username is already taken." });

            if (await _userManager.FindByEmailAsync(request.Email) != null)
                return IdentityResult.Failed(new IdentityError { Description = "Email is already in use." });

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return result;

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                    return roleResult;
            }

            var roleAssignmentResult = await _userManager.AddToRoleAsync(user, roleName);
            return roleAssignmentResult.Succeeded ? result : roleAssignmentResult;
        }
    }
}
