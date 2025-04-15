using Ardalis.Result;
using AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Create;
using AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.RiskControls;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class RiskControlMatrixController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Risk Control Matrix
    [HttpPost("create-risk-control-matrix")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRiskControlMatrixCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRiskControlMatrix([FromBody] CreateRiskControlMatrixCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Risk Control Matrix
    [HttpPut("update-risk-control-matrix")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRiskControlMatrix([FromBody] UpdateRiskControlMatrixCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}