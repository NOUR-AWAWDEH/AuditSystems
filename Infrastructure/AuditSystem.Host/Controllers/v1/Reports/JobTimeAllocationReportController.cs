using AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Create;
using AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Reports;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class JobTimeAllocationReportController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Job Time Allocation Report
    [HttpPost("create-job-time-allocation-report")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateJobTimeAllocationReportCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateJobTimeAllocationReport([FromBody] CreateJobTimeAllocationReportCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Job Time Allocation Report
    [HttpPut("update-job-time-allocation-report")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobTimeAllocationReport([FromBody] UpdateJobTimeAllocationReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync(command);
}