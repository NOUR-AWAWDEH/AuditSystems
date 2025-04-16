using Ardalis.Result;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Create;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Delete;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Update;
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

    //Delete Job Prioritization
    [HttpDelete("delete-job-prioritization")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobPrioritization([FromBody] DeleteJobPrioritizationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}