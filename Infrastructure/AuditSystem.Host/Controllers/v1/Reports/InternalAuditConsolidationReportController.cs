using AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Create;
using AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Reports;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class InternalAuditConsolidationReportController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Internal Audit Consolidated Report
    [HttpPost("create-internal-audit-consolidated-report")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateInternalAuditConsolidationReportCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateInternalAuditConsolidatedReport([FromBody] CreateInternalAuditConsolidationReportCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Internal Audit Consolidated Report
    [HttpPut("update-internal-audit-consolidated-report")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateInternalAuditConsolidatedReport([FromBody] UpdateInternalAuditConsolidationReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync(command);
}