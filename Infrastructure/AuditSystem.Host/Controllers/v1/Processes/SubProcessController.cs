using Ardalis.Result;
using AuditSystem.Application.Features.Processes.SubProcess.Create;
using AuditSystem.Application.Features.Processes.SubProcess.Delete;
using AuditSystem.Application.Features.Processes.SubProcess.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Processes;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class SubProcessController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Sub Process
    [HttpPost("create-sub-process")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateSubProcessCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditSubProcess([FromBody] CreateSubProcessCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Sub Process
    [HttpPut("update-sub-process")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditSubProcess([FromBody] UpdateSubProcessCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Delete Sub Process
    [HttpDelete("delete-sub-process")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditSubProcess([FromBody] DeleteSubProcessCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}