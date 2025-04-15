using Ardalis.Result;
using AuditSystem.Application.Features.Audit.AuditPlanSummary.Create;
using AuditSystem.Application.Features.Audit.AuditPlanSummary.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditPlanSummaryController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Plan Summary
    [HttpPost("create-audit-plan-summary")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditPlanSummaryCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditPlanSummary([FromBody] CreateAuditPlanSummaryCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Plan Summary
    [HttpPut("update-audit-plan-summary")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditPlanSummary([FromBody] UpdateAuditPlanSummaryCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}