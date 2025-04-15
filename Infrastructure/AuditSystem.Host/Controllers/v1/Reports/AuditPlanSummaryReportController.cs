using Ardalis.Result;
using AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Create;
using AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Reports;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditPlanSummaryReportController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Plan Summary Report
    [HttpPost("create-audit-plan-summary-report")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditPlanSummaryReportCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditPlanSummaryReport([FromBody] CreateAuditPlanSummaryReportCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Plan Summary Report
    [HttpPut("update-audit-plan-summary-report")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditPlanSummaryReport([FromBody] UpdateAuditPlanSummaryReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}