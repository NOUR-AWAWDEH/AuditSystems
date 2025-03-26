using AuditSystem.Application.Features.Risks.Risk.Create;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Risks
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public sealed class RiskController(IMediator mediator) : ApiControllerBase(mediator)
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateRiskCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRisk([FromBody] CreateRiskCommand command) =>
        await ProcessRequestToActionResultAsync(command);
    }
}