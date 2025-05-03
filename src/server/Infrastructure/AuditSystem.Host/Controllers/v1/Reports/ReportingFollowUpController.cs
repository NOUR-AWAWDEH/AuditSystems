using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Reports.ReportingFollowUp.Create;
using AuditSystem.Application.Features.Reports.ReportingFollowUp.Delete;
using AuditSystem.Application.Features.Reports.ReportingFollowUp.Get;
using AuditSystem.Application.Features.Reports.ReportingFollowUp.Search;
using AuditSystem.Application.Features.Reports.ReportingFollowUp.Update;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Reports;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class ReportingFollowUpController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Reporting Follow Up
    [HttpPost("create-reporting-follow-up")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateReportingFollowUpCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateReportingFollowUpReport([FromBody] CreateReportingFollowUpCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Reporting Follow Up
    [HttpPut("update-reporting-follow-up")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateReportingFollowUp([FromBody] UpdateReportingFollowUpCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Reporting Follow Up
    [HttpGet("get-reporting-follow-up")]
    [ProducesResponseType(typeof(ApiResponse<GetReportingFollowUpByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportingFollowUp([FromQuery] GetReportingFollowUpByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Reporting Follow Up
    [HttpDelete("delete-reporting-follow-up")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReportingFollowUp([FromBody] DeleteReportingFollowUpCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Reporting Follow Up
    [HttpGet("search-reporting-follow-up")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<ReportingFollowUpModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchReportingFollowUp([FromQuery] SearchReportingFollowUpQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}