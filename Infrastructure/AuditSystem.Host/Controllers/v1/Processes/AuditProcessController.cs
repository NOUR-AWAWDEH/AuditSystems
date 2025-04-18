using Ardalis.Result;
using AuditSystem.Application.Features.Processes.AuditProcess.Create;
using AuditSystem.Application.Features.Processes.AuditProcess.Delete;
using AuditSystem.Application.Features.Processes.AuditProcess.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Processes;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditProcessController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Process
    [HttpPost("create-audit-process")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditProcessCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditProcess([FromBody] CreateAuditProcessCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Process
    [HttpPut("update-audit-process")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditProcess([FromBody] UpdateAuditProcessCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Delete Audit Process
    [HttpDelete("delete-audit-process")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditProcess([FromBody] DeleteAuditProcessCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}