using AuditSystem.Application.Features.Audit.Objective.Create;
using AuditSystem.Application.Features.Audit.Objective.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class ObjectiveController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Objective
    [HttpPost("create-objective")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateObjectiveCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateObjective([FromBody] CreateObjectiveCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Objective
    [HttpPut("update-objective")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateObjective([FromBody] UpdateObjectiveCommand command) =>
        await ProcessRequestToActionNoContentResultAsync(command);
}