using AuditSystem.Contract.Models.RateLimiting;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers;

public static class RateLimitHelper
{
    private const int BaseBlockMinutes = 5;

    public static bool IsBlocked(BruteForceProtectionEntry entry, ILogger logger, string ip)
    {
        var isBlocked = DateTime.UtcNow < entry.BlockedUntil;
        if (isBlocked)
        {
            logger?.LogWarning("Blocked login attempt from {IP}. Block expires at {Expiry}", 
                ip, entry.BlockedUntil);
        }
        return isBlocked;
    }

    public static void ApplyBlock(BruteForceProtectionEntry entry)
    {
        entry.BlockCount++;
        var blockDuration = TimeSpan.FromMinutes(
            BaseBlockMinutes * Math.Pow(2, Math.Min(entry.BlockCount - 1, 5)));
        entry.BlockedUntil = DateTime.UtcNow.Add(blockDuration);
        entry.Requests = 0;
    }
}