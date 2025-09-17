using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Processes.SubProcess.Create;
using AuditSystem.Application.Features.Processes.SubProcess.Delete;
using AuditSystem.Application.Features.Processes.SubProcess.Search;
using AuditSystem.Application.Features.Processes.SubProcess.Update;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.Application.Features.Processes.SubProcess.Get.GetById;
using AuditSystem.Application.Features.Processes.SubProcess.Get.GetByUserId;

namespace AuditSystem.Host.Controllers.v1.Processes;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class SubProcessController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Sub Process
    [HttpPost("create-sub-process")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateSubProcessCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSubProcess([FromBody] CreateSubProcessCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Sub Process
    [HttpPut("update-sub-process")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSubProcess([FromBody] UpdateSubProcessCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Sub Process By Id
    [HttpGet("get-sub-process-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetSubProcessByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubProcess([FromQuery] GetSubProcessByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Get Sub Process By User Id
    [HttpGet("get-sub-processes-by-user-id")]
    [ProducesResponseType(typeof(ApiResponse<GetSubProcessesByUserIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubProcess([FromQuery] GetSubProcessesByUserIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    
    //Delete Sub Process
    [HttpDelete("delete-sub-process-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSubProcess([FromBody] DeleteSubProcessCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Sub Process
    [HttpGet("search-sub-process-by-user-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<SubProcessModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchSubProcess([FromQuery] SearchSubProcessQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}