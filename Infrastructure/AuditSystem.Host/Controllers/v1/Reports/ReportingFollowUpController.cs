using Ardalis.Result;
using AuditSystem.Application.Features.Reports.ReportingFollowUp.Create;
using AuditSystem.Application.Features.Reports.ReportingFollowUp.Delete;
using AuditSystem.Application.Features.Reports.ReportingFollowUp.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Reports;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class ReportingFollowUpController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Reporting Follow Up
    [HttpPost("create-reporting-follow-up")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateReportingFollowUpCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateReportingFollowUpReport([FromBody] CreateReportingFollowUpCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Reporting Follow Up
    [HttpPut("update-reporting-follow-up")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateReportingFollowUp([FromBody] UpdateReportingFollowUpCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Delete Reporting Follow Up
    [HttpDelete("delete-reporting-follow-up")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReportingFollowUp([FromBody] DeleteReportingFollowUpCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}