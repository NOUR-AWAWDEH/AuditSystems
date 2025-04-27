using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Create;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Delete;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Get;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Search;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Update;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Jobs;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class JobPrioritizationController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Job Prioritization
    [HttpPost("create-job-prioritization")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateJobPrioritizationCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateJobPrioritization([FromBody] CreateJobPrioritizationCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Job Prioritization
    [HttpPut("update-job-prioritization")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobPrioritization([FromBody] UpdateJobPrioritizationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Job Prioritization
    [HttpGet("get-job-prioritization")]
    [ProducesResponseType(typeof(ApiResponse<GetJobPrioritizationByIdQuery>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobPrioritization([FromQuery] GetJobPrioritizationByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Job Prioritization
    [HttpDelete("delete-job-prioritization")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobPrioritization([FromBody] DeleteJobPrioritizationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
        //Search Job Prioritization
    [HttpGet("search-job-prioritization")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<JobPrioritizationModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchJobPrioritization([FromQuery] SearchJobPrioritizationQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}