using AuditSystem.Application.Features.Risks.Commands.CreateRisk;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class RiskController(IMediator mediator) : ApiControllerBase(mediator)
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateRiskCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTenant([FromBody] CreateRiskCommand command) =>
        await ProcessRequestToActionResultAsync(command);
    }
}
