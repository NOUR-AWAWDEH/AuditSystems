using AuditSystem.Contract.Models.RateLimiting;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Logging;

public static class RateLimitLogger
{
    public static void LogRequest(ILogger logger, string ip, BruteForceProtectionEntry entry)
    {
        logger?.LogDebug("Login attempt #{Attempt} from {IP}", entry.Requests, ip);
    }

    public static void LogBlockedAttempt(ILogger logger, string ip, BruteForceProtectionEntry entry)
    {
        logger?.LogWarning("Blocked login attempt from {IP}. Block #{BlockCount} expires at {Expiry}", 
            ip, entry.BlockCount, entry.BlockedUntil);
    }

    public static void LogRateLimitApplied(ILogger logger, string ip, BruteForceProtectionEntry entry)
    {
        logger?.LogWarning("Rate limit applied to {IP}. Block #{BlockCount} until {Expiry}", 
            ip, entry.BlockCount, entry.BlockedUntil);
    }
}