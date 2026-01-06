using AuditSystem.Application.Features.Reports.DriftReport.Create;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.Contract.Common;

namespace AuditSystem.Host.Controllers.v1.Reports;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class DriftReportController(IMediator mediator) : ApiControllerBase(mediator)
{
    // -----------------------------
    // Create Drift Report
    // -----------------------------
    [HttpPost("create-drift-report")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateDriftReportCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateDriftReport([FromBody] CreateDriftReportCommand command) =>
        await ProcessRequestToActionResultAsync(command);
}