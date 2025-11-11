namespace AuditSystem.Host.Seed.Interfaces;

using AuditSystem.Domain.Entities.Users;

public interface IAdminUserSeeder
{
    Task<User?> SeedAdminUserAsync(string email, string username, string password, Guid companyId);
}