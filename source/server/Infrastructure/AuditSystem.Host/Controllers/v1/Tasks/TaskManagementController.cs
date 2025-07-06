using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Tasks.Create;
using AuditSystem.Application.Features.Tasks.Delete;
using AuditSystem.Application.Features.Tasks.Get;
using AuditSystem.Application.Features.Tasks.Search;
using AuditSystem.Application.Features.Tasks.Update;
using AuditSystem.Contract.Models.Tasks;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AuditSystem.Host.Controllers.v1.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

    //Get TaskManagement
    [HttpGet("get-task-management")]
    [ProducesResponseType(typeof(ApiResponse<GetTaskManagementByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTaskManagement([FromQuery] GetTaskManagementByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete TaskManagement
    [HttpDelete("delete-task-management")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTaskManagement([FromBody] DeleteTaskManagementCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search TaskManagement
    [HttpGet("search-task-management")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<TaskManagementModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchTaskManagement([FromQuery] SearchTaskManagementQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}