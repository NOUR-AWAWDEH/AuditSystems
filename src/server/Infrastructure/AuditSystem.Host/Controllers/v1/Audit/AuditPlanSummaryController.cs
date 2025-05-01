using Ardalis.Result;
using AuditSystem.Application.Features.Audit.AuditPlanSummary.Create;
using AuditSystem.Application.Features.Audit.AuditPlanSummary.Delete;
using AuditSystem.Application.Features.Audit.AuditPlanSummary.Get;
using AuditSystem.Application.Features.Audit.AuditPlanSummary.Search;
using AuditSystem.Application.Features.Audit.AuditPlanSummary.Update;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Host.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditPlanSummaryController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Plan Summary
    [HttpPost("create-audit-plan-summary")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditPlanSummaryCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditPlanSummary([FromBody] CreateAuditPlanSummaryCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Plan Summary
    [HttpPut("update-audit-plan-summary")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditPlanSummary([FromBody] UpdateAuditPlanSummaryCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Audit Plan Summary
    [HttpGet("get-audit-plan-summary")]
    [ProducesResponseType(typeof(ApiResponse<GetAuditPlanSummaryByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuditPlanSummary([FromQuery] GetAuditPlanSummaryByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Audit Plan Summary
    [HttpDelete("delete-audit-plan-summary")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditPlanSummary([FromBody] DeleteAuditPlanSummaryCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Audit Plan Summary
    [HttpGet("search-audit-plan-summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PaginationRequestResponse<AuditPlanSummaryModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAuditPlanSummary([FromQuery] SearchAuditPlanSummaryQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}