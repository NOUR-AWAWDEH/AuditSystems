using Ardalis.Result;
using AuditSystem.Application.Features.Audit.AuditEngagement.Create;
using AuditSystem.Application.Features.Audit.AuditEngagement.Delete;
using AuditSystem.Application.Features.Audit.AuditEngagement.Get;
using AuditSystem.Application.Features.Audit.AuditEngagement.Search;
using AuditSystem.Application.Features.Audit.AuditEngagement.Update;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditEngagementController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Engagement
    [HttpPost("create-audit-engagement")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditEngagementCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditEngagement([FromBody] CreateAuditEngagementCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Engagement
    [HttpPut("update-audit-engagement")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditEngagement([FromBody] UpdateAuditEngagementCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Audit Engagement
    [HttpGet("get-audit-engagement")]
    [ProducesResponseType(typeof(ApiResponse<GetAuditEngagementByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuditEngagement([FromQuery] GetAuditEngagementByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Audit Engagement
    [HttpDelete("delete-audit-engagement")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditEngagement([FromBody] DeleteAuditEngagementCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Audit Engagement
    [HttpGet("search-audit-engagement")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PaginationRequestResponse<AuditEngagementModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAuditEngagement([FromQuery] SearchAuditEngagementQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}