using AuditSystem.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AuditSystem.BusinessLogic.Attributes;

public sealed class AutoInjectUserIdAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userId = GetCurrentUserId(context);
        if (userId == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Find all IUserAwareRequest parameters and inject the UserId
        foreach (var parameter in context.ActionArguments.Values)
        {
            if (parameter is IUserAwareRequest userAwareRequest)
            {
                userAwareRequest.UserId = userId.Value;
            }
        }

        base.OnActionExecuting(context);
    }

    private static Guid? GetCurrentUserId(ActionExecutingContext context)
    {
        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}