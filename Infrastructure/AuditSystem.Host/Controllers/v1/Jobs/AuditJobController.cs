using Ardalis.Result;
using AuditSystem.Application.Features.Jobs.AuditJob.Create;
using AuditSystem.Application.Features.Jobs.AuditJob.Delete;
using AuditSystem.Application.Features.Jobs.AuditJob.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Jobs;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditJobController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Job
    [HttpPost("create-audit-job")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditJobCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditJob([FromBody] CreateAuditJobCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Job
    [HttpPut("update-audit-job")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditJob([FromBody] UpdateAuditJobCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Delete Audit Job
    [HttpDelete("delete-audit-job")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditJob([FromBody] DeleteAuditJobCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}