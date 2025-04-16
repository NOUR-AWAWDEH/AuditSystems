using Ardalis.Result;
using AuditSystem.Application.Features.Audit.AuditUniverse.Create;
using AuditSystem.Application.Features.Audit.AuditUniverse.Delete;
using AuditSystem.Application.Features.Audit.AuditUniverse.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditUniverseController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Universe
    [HttpPost("create-audit-universe")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditUniverseCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditUniverse([FromBody] CreateAuditUniverseCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Universe
    [HttpPut("update-audit-universe")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditUniverse([FromBody] UpdateAuditUniverseCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Delete Audit Universe
    [HttpDelete("delete-audit-universe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditUniverse([FromBody] DeleteAuditUniverseCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}