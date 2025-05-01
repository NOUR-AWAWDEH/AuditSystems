using Ardalis.Result;
using AuditSystem.Application.Features.Checklists.Checklist.Create;
using AuditSystem.Application.Features.Checklists.Checklist.Delete;
using AuditSystem.Application.Features.Checklists.Checklist.Get;
using AuditSystem.Application.Features.Checklists.Checklist.Update;
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
public sealed class ChecklistController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Checklist
    [HttpPost("create-checklist")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateChecklistCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateChecklist([FromBody] CreateChecklistCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Checklist
    [HttpPut("update-checklist")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateChecklist([FromBody] UpdateChecklistCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Checklist
    [HttpGet("get-checklist")]
    [ProducesResponseType(typeof(ApiResponse<GetChecklistByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetChecklist([FromQuery] GetChecklistByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Checklist
    [HttpDelete("delete-checklist")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteChecklist([FromBody] DeleteChecklistCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Checklist
    [HttpGet("search-checklist")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<PaginationRequestResponse<ChecklistModel>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchChecklist([FromQuery] GetChecklistByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}