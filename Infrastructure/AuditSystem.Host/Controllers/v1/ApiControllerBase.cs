using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuditSystem.Host.Responses;
using Ardalis.Result;

namespace AuditSystem.Host.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private readonly IMediator _mediator;

    protected ApiControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected async Task<IActionResult> ProcessRequestToActionResultAsync<TResponse>(IRequest<Result<TResponse>> request)
    {
        var response = await _mediator.Send(request);

        if (!response.IsSuccess)
            return BadRequest(ApiResponse<TResponse>.BadRequest(response.Errors.Select(e => new ApiErrorResponse(e))));

        return Ok(ApiResponse<TResponse>.Ok(response.Value!, "Request processed successfully"));
    }

    protected async Task<IActionResult> ProcessRequestToActionNoContentResultAsync(
        IRequest<Result> request,
        string successMessage = "Operation completed successfully")
    {
        var response = await _mediator.Send(request);

        if (!response.IsSuccess)
            return BadRequest(ApiResponse.BadRequest(response.Errors.Select(e => new ApiErrorResponse(e))));

        return Ok(ApiResponse.Ok(successMessage));
    }
}