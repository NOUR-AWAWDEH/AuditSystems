using Ardalis.Result;
using AuditSystem.Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AuditSystem.Contract.Common;

namespace AuditSystem.Host.Controllers.v1;

public abstract class ApiControllerBase(IMediator mediator) : ControllerBase
{
    public async Task<IActionResult> ProcessRequestToActionResultAsync<TResponse>(IRequest<Result<TResponse>> request)
    {
        var response = await mediator.Send(request);
        return response.ToActionResult();
    }

    public async Task<IActionResult> ProcessRequestToActionNoContentResultAsync<TResponse>(IRequest<Result> request)
    {
        var response = await mediator.Send(request);
        return response.ToActionResult();
    }

    protected Guid? GetCurrentUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            return null;
        }
        return userGuid;
    }

    protected IActionResult? ValidateCurrentUser(out Guid userId)
    {
        var userIdNullable = GetCurrentUserId();
        if (!userIdNullable.HasValue)
        {
            userId = Guid.Empty;
            return Unauthorized(ApiResponse.Unauthorized("User not found or invalid"));
        }
        userId = userIdNullable.Value;
        return null;
    }
}