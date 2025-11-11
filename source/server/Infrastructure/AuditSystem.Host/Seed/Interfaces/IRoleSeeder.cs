namespace AuditSystem.Host.Seed.Interfaces;

public interface IRoleSeeder
{
    Task SeedRolesAsync(CancellationToken cancellationToken = default);
}
