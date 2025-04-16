using Ardalis.Result;
using AuditSystem.Application.Features.Organisation.SubLocation.Create;
using AuditSystem.Application.Features.Organisation.SubLocation.Update;
using AuditSystem.Application.Features.Organization.SubLocation.Delete;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Organization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class SubLocationController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create SubLocation
    [HttpPost("create-sub-location")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateSubLocationCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSubLocation([FromBody] CreateSubLocationCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Sub Location
    [HttpPut("update-sub-location")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSubLocation([FromBody] UpdateSubLocationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Delete Sub Location
    [HttpDelete("delete-sub-location")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSubLocation([FromBody] DeleteSubLocationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}