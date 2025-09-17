using Microsoft.Extensions.Caching.Memory;

namespace AuditSystem.BusinessLogic.Factories;

public static class CacheOptionsFactory
{
    public static MemoryCacheEntryOptions CreateBruteForceProtectionOptions()
    {
        return new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        };
    }
}