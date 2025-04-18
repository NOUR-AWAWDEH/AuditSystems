using Ardalis.Result;
using AuditSystem.Application.Features.Organization.Location.Create;
using AuditSystem.Application.Features.Organization.Location.Update;
using AuditSystem.Application.Features.Organization.Location.Delete;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Organization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class LocationController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Location
    [HttpPost("create-location")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateLocationCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateLocation([FromBody] CreateLocationCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Location
    [HttpPut("update-location")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLocation([FromBody] UpdateLocationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Delete Location
    [HttpDelete("delete-location")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLocation([FromBody] DeleteLocationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}