using Ardalis.Result;
using AuditSystem.Application.Features.Checklists.Remark.Create;
using AuditSystem.Application.Features.Checklists.Remark.Delete;
using AuditSystem.Application.Features.Checklists.Remark.Get;
using AuditSystem.Application.Features.Checklists.Remark.Search;
using AuditSystem.Application.Features.Checklists.Remark.Update;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Host.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.CheckLists;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class RemarkController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Remark
    [HttpPost("create-remark")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRemarkCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRemark([FromBody] CreateRemarkCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Remark
    [HttpPut("update-remark")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRemark([FromBody] UpdateRemarkCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Remark
    [HttpGet("get-remark")]
    [ProducesResponseType(typeof(ApiResponse<GetRemarkByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRemark([FromQuery] GetRemarkByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Remark
    [HttpDelete("delete-remark")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRemark([FromBody] DeleteRemarkCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Search Remark
    [HttpGet("search-remark")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<RemarkModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchRemark([FromQuery] SearchRemarkQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}