using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Host.Seed.Interfaces;

public interface IRoleAssignmentService
{
    Task AssignAdminRoleAsync(User user);
}