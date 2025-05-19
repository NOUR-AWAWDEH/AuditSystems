using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Create;
using AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Delete;
using AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Get;
using AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Search;
using AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Update;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AuditSystem.Host.Controllers.v1.Reports;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Job Time Allocation Report
    [HttpGet("get-job-time-allocation-report")]
    [ProducesResponseType(typeof(ApiResponse<GetJobTimeAllocationReportByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobTimeAllocationReport([FromQuery] GetJobTimeAllocationReportByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    //Delete Job Time Allocation Report
    [HttpDelete("delete-job-time-allocation-report")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobTimeAllocationReport([FromBody] DeleteJobTimeAllocationReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Job Time Allocation Report
    [HttpGet("search-job-time-allocation-report")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<JobTimeAllocationReportModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchJobTimeAllocationReport([FromQuery] SearchJobTimeAllocationReportQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}