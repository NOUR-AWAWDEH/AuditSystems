using AuditSystem.Application.Features.Audit.AuditEngagement.Create;
using AuditSystem.Application.Features.Audit.AuditEngagement.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditEngagementController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Engagement
    [HttpPost("create-audit-engagement")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditEngagementCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditEngagement([FromBody] CreateAuditEngagementCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Engagement
    [HttpPut("update-audit-engagement")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditEngagement([FromBody] UpdateAuditEngagementCommand command) =>
        await ProcessRequestToActionNoContentResultAsync(command);
}