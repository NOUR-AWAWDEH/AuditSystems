using Ardalis.Result;
using AuditSystem.Application.Features.Audit.BusinessObjective.Create;
using AuditSystem.Application.Features.Audit.BusinessObjective.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class BusinessObjectiveController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Business Objective
    [HttpPost("create-business-objective")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateBusinessObjectiveCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBusinessObjective([FromBody] CreateBusinessObjectiveCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Business Objective
    [HttpPut("update-business-objective")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBusinessObjective([FromBody] UpdateBusinessObjectiveCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}