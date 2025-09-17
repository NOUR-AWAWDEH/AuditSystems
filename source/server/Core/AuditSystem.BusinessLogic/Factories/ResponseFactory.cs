using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.BusinessLogic.Factories;

public static class ResponseFactory
{
    public static IActionResult CreateBlockedResponse(DateTime blockedUntil)
    {
        var remainingTime = blockedUntil - DateTime.UtcNow;
        return new ContentResult
        {
            ContentType = "application/json",
            StatusCode = 429,
            Content = JsonSerializer.Serialize(new
            {
                error = "too_many_requests",
                message = "Too many request attempts. Please try again later.",
                retry_after_seconds = (int)remainingTime.TotalSeconds
            })
        };
    }
}