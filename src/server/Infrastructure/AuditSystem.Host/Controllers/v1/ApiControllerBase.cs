using Ardalis.Result;
using AuditSystem.Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
}