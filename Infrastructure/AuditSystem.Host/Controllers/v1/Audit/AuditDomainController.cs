using AuditSystem.Application.Features.Audit.AuditDomain.Create;
using AuditSystem.Application.Features.Audit.AuditDomain.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditDomainController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Domain
    [HttpPost("create-audit-domain")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditDomainCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditDomain([FromBody] CreateAuditDomainCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Domain
    [HttpPut("update-audit-domain")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTenant([FromBody] UpdateAuditDomainCommand command) =>
    await ProcessRequestToActionNoContentResultAsync(command);
}