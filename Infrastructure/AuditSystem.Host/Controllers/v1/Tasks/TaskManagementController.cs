using Ardalis.Result;
using AuditSystem.Application.Features.Tasks.Create;
using AuditSystem.Application.Features.Tasks.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class TaskManagementController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create TaskManagement
    [HttpPost("create-task-management")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateTaskManagementCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTaskManagement([FromBody] CreateTaskManagementCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update TaskManagement
    [HttpPut("update-task-management")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTaskManagement([FromBody] UpdateTaskManagementCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}