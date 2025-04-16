using Ardalis.Result;
using AuditSystem.Application.Features.Risks.RiskFactor.Create;
using AuditSystem.Application.Features.Risks.RiskFactor.Delete;
using AuditSystem.Application.Features.Risks.RiskFactor.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Risks;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class RiskFactorController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Risk Factor
    [HttpPost("create-risk-factor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRiskFactorCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRiskFactor([FromBody] CreateRiskFactorCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Risk Factor
    [HttpPut("update-risk-factor")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRiskFactor([FromBody] UpdateRiskFactorCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Delete Risk Factor
    [HttpDelete("delete-risk-factor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRiskFactor([FromBody] DeleteRiskFactorCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}