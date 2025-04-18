using Ardalis.Result;
using AuditSystem.Application.Features.RiskControls.RiskControls.Create;
using AuditSystem.Application.Features.RiskControls.RiskControls.Delete;
using AuditSystem.Application.Features.RiskControls.RiskControls.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.RiskControls;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class RiskControlsController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Risk Controls
    [HttpPost("create-risk-controls")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRiskControlsCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRiskControls([FromBody] CreateRiskControlsCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Risk Controls
    [HttpPut("update-risk-controls")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRiskControls([FromBody] UpdateRiskControlsCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Delete Risk Controls
    [HttpDelete("delete-risk-controls")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRiskControls([FromBody] DeleteRiskControlsCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}