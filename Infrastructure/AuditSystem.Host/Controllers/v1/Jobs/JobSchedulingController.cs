using Ardalis.Result;
using AuditSystem.Application.Features.Jobs.JobScheduling.Create;
using AuditSystem.Application.Features.Jobs.JobScheduling.Delete;
using AuditSystem.Application.Features.Jobs.JobScheduling.Update;
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

    //Delete Job Scheduling
    [HttpDelete("delete-job-scheduling")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobScheduling([FromBody] DeleteJobSchedulingCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}