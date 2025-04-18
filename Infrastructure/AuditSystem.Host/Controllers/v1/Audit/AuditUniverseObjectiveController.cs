using Ardalis.Result;
using AuditSystem.Application.Features.Audit.AuditUniverseObjective.Create;
using AuditSystem.Application.Features.Audit.AuditUniverseObjective.Delete;
using AuditSystem.Application.Features.Audit.AuditUniverseObjective.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditUniverseObjectiveController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Universe Objective
    [HttpPost("create-audit-universe-objective")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditUniverseObjectiveCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditUniverseObjective([FromBody] CreateAuditUniverseObjectiveCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Universe Objective
    [HttpPut("update-audit-universe-objective")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditUniverseObjective([FromBody] UpdateAuditUniverseObjectiveCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Delete Audit Universe Objective
    [HttpDelete("delete-audit-universe-objective")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

    public async Task<IActionResult> DeleteAuditUniverseObjective([FromBody] DeleteAuditUniverseObjectiveCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}