using Ardalis.Result;
using AuditSystem.Application.Features.Reports.AuditExceptionReport.Create;
using AuditSystem.Application.Features.Reports.AuditExceptionReport.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Reports;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditExceptionReportController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Report
    [HttpPost("create-audit-exception-report")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditExceptionReportCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateReport([FromBody] CreateAuditExceptionReportCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Report
    [HttpPut("update-audit-exception-report")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateReport([FromBody] UpdateAuditExceptionReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}