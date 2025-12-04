using AuditSystem.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks; // Added for Task

namespace AuditSystem.BusinessLogic.Attributes;

public sealed class AutoInjectUserIdAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<AutoInjectUserIdAttribute>)) as ILogger<AutoInjectUserIdAttribute>;

        logger?.LogInformation("AutoInjectUserId: Starting user ID injection");

        var userId = GetCurrentUserId(context, logger);
        if (userId == null)
        {
            logger?.LogWarning("AutoInjectUserId: No user ID found in claims, returning Unauthorized");
            context.Result = new UnauthorizedResult();
            return;
        }

        logger?.LogInformation("AutoInjectUserId: Found user ID {UserId}", userId.Value);

        // Find all IUserAwareRequest parameters and inject the UserId
        int injectedCount = 0;
        foreach (var parameter in context.ActionArguments.Values)
        {
            if (parameter is IUserAwareRequest userAwareRequest)
            {
                userAwareRequest.UserId = userId.Value;
                injectedCount++;
                logger?.LogInformation("AutoInjectUserId: Injected UserId into {ParameterType}", parameter.GetType().Name);
            }
        }

        logger?.LogInformation("AutoInjectUserId: Injected UserId into {Count} parameters", injectedCount);

        // Continue with the action execution
        await next();
    }

    private static Guid? GetCurrentUserId(ActionExecutingContext context, ILogger? logger)
    {
        var claims = context.HttpContext.User.Claims.ToList();
        logger?.LogInformation("AutoInjectUserId: Found {Count} claims", claims.Count);

        foreach (var claim in claims)
        {
            logger?.LogDebug("AutoInjectUserId: Claim Type={Type}, Value={Value}", claim.Type, claim.Value);
        }

        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        logger?.LogInformation("AutoInjectUserId: NameIdentifier claim value: {Value}", userIdClaim ?? "(null)");

        if (Guid.TryParse(userIdClaim, out var userId))
        {
            logger?.LogInformation("AutoInjectUserId: Successfully parsed UserId: {UserId}", userId);
            return userId;
        }

        logger?.LogWarning("AutoInjectUserId: Failed to parse UserId from claim value: {Value}", userIdClaim ?? "(null)");
        return null;
    }
}