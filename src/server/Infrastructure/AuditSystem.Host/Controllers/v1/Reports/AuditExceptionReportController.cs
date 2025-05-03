using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Reports.AuditExceptionReport.Create;
using AuditSystem.Application.Features.Reports.AuditExceptionReport.Delete;
using AuditSystem.Application.Features.Reports.AuditExceptionReport.Get;
using AuditSystem.Application.Features.Reports.AuditExceptionReport.Search;
using AuditSystem.Application.Features.Reports.AuditExceptionReport.Update;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Reports;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditExceptionReportController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Report
    [HttpPost("create-audit-exception-report")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditExceptionReportCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateReport([FromBody] CreateAuditExceptionReportCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Report
    [HttpPut("update-audit-exception-report")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateReport([FromBody] UpdateAuditExceptionReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Report
    [HttpGet("get-audit-exception-report")]
    [ProducesResponseType(typeof(ApiResponse<GetAuditExceptionReportByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReport([FromQuery] GetAuditExceptionReportByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Report
    [HttpDelete("delete-audit-exception-report")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReport([FromBody] DeleteAuditExceptionReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Report
    [HttpGet("search-audit-exception-report")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<AuditExceptionReportModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchReport([FromQuery] SearchAuditExceptionReportQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}