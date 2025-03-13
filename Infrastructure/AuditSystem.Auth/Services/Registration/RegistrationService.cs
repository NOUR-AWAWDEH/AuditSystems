using AuditSystem.Auth.Dtos;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Services.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager; // Use custom Role, not IdentityRole

        public RegistrationService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task RegisterAsync(RegisterDto request)
        {
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                // Assign a default role (e.g., "User")
                if (!await _roleManager.RoleExistsAsync(Role.User))
                {
                    await _roleManager.CreateAsync(new Role { Name = Role.User });
                }
                await _userManager.AddToRoleAsync(user, Role.User);
            }
            else
            {
                throw new Exception("User creation failed");
            }
        }
    }
}