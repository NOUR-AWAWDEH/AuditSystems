using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Jobs.JobScheduling.Create;
using AuditSystem.Application.Features.Jobs.JobScheduling.Delete;
using AuditSystem.Application.Features.Jobs.JobScheduling.Get;
using AuditSystem.Application.Features.Jobs.JobScheduling.Search;
using AuditSystem.Application.Features.Jobs.JobScheduling.Update;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Jobs;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class JobSchedulingController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Job Scheduling
    [HttpPost("create-job-scheduling")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateJobSchedulingCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateJobScheduling([FromBody] CreateJobSchedulingCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Job Scheduling
    [HttpPut("update-job-scheduling")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobScheduling([FromBody] UpdateJobSchedulingCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Job Scheduling
    [HttpGet("get-job-scheduling")]
    [ProducesResponseType(typeof(ApiResponse<GetJobSchedulingByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobScheduling([FromQuery] GetJobSchedulingByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Job Scheduling
    [HttpDelete("delete-job-scheduling")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobScheduling([FromBody] DeleteJobSchedulingCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Job Scheduling
    [HttpGet("search-job-scheduling")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<JobSchedulingModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchJobScheduling([FromQuery] SearchJobSchedulingQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}