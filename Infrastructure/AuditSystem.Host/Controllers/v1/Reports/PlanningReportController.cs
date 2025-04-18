using Ardalis.Result;
using AuditSystem.Application.Features.Reports.PlanningReport.Create;
using AuditSystem.Application.Features.Reports.PlanningReport.Delete;
using AuditSystem.Application.Features.Reports.PlanningReport.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Reports;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class PlanningReportController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Planning Report
    [HttpPost("create-planning-report")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreatePlanningReportCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePlanningReport([FromBody] CreatePlanningReportCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Planning Report
    [HttpPut("update-planning-report")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePlanningReport([FromBody] UpdatePlanningReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Delete Planning Report
    [HttpDelete("delete-planning-report")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePlanningReport([FromBody] DeletePlanningReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}