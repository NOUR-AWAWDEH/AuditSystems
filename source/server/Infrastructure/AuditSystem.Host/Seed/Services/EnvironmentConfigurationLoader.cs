namespace AuditSystem.Host.Seed.Services;

using Models;
using Exceptions;

public interface IConfigurationLoader
{
    AdminSeedConfiguration LoadAdminConfiguration();
}

public sealed class EnvironmentConfigurationLoader : IConfigurationLoader
{
    public AdminSeedConfiguration LoadAdminConfiguration()
    {
        return new AdminSeedConfiguration
        {
            Email = GetRequiredEnvironmentVariable("ADMIN_INITIAL_EMAIL"),
            Password = GetRequiredEnvironmentVariable("ADMIN_INITIAL_PASSWORD"),
            Username = GetRequiredEnvironmentVariable("ADMIN_INITIAL_USERNAME"),
            CompanyId = GetRequiredGuidEnvironmentVariable("ADMIN_INITIAL_COMPANY_ID")
        };
    }

    private static string GetRequiredEnvironmentVariable(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName);
        return string.IsNullOrWhiteSpace(value)
            ? throw new SeedingException($"{variableName} is not set or empty")
            : value.Trim();
    }

    private static Guid GetRequiredGuidEnvironmentVariable(string variableName)
    {
        var value = GetRequiredEnvironmentVariable(variableName);
        return Guid.TryParse(value, out var guid)
            ? guid
            : throw new SeedingException($"{variableName} is not a valid GUID: {value}");
    }
}