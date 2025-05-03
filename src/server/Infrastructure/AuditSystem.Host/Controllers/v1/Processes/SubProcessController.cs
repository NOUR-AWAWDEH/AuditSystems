using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Processes.SubProcess.Create;
using AuditSystem.Application.Features.Processes.SubProcess.Delete;
using AuditSystem.Application.Features.Processes.SubProcess.Get;
using AuditSystem.Application.Features.Processes.SubProcess.Search;
using AuditSystem.Application.Features.Processes.SubProcess.Update;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Processes;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class SubProcessController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Sub Process
    [HttpPost("create-sub-process")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateSubProcessCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditSubProcess([FromBody] CreateSubProcessCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Sub Process
    [HttpPut("update-sub-process")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditSubProcess([FromBody] UpdateSubProcessCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Sub Process
    [HttpGet("get-sub-process")]
    [ProducesResponseType(typeof(ApiResponse<GetSubProcessByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuditSubProcess([FromQuery] GetSubProcessByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    //Delete Sub Process
    [HttpDelete("delete-sub-process")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditSubProcess([FromBody] DeleteSubProcessCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Sub Process
    [HttpGet("search-sub-process")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<SubProcessModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAuditSubProcess([FromQuery] SearchSubProcessQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}