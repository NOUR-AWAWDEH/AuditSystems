using Microsoft.AspNetCore.Http;

namespace AuditSystem.BusinessLogic.Helpers;

public static class IpHelper
{
    public static string GetClientIp(HttpContext context)
    {
        // Consider using X-Forwarded-For if behind a proxy
        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}