using Ardalis.Result;
using AuditSystem.Application.Features.RiskControls.RiskProgram.Create;
using AuditSystem.Application.Features.RiskControls.RiskProgram.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.RiskControls;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class RiskProgramController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Risk Program
    [HttpPost("create-risk-program")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRiskProgramCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRiskProgram([FromBody] CreateRiskProgramCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Risk Program
    [HttpPut("update-risk-program")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRiskProgram([FromBody] UpdateRiskProgramCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}