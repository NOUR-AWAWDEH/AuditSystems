using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Create;
using AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Delete;
using AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Get;
using AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Search;
using AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Update;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AuditSystem.Host.Controllers.v1.Reports;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class InternalAuditConsolidationReportController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Internal Audit Consolidated Report
    [HttpPost("create-internal-audit-consolidated-report")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateInternalAuditConsolidationReportCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateInternalAuditConsolidatedReport([FromBody] CreateInternalAuditConsolidationReportCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Internal Audit Consolidated Report
    [HttpPut("update-internal-audit-consolidated-report")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateInternalAuditConsolidatedReport([FromBody] UpdateInternalAuditConsolidationReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Internal Audit Consolidated Report
    [HttpGet("get-internal-audit-consolidated-report")]
    [ProducesResponseType(typeof(ApiResponse<GetInternalAuditConsolidationReportByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInternalAuditConsolidatedReport([FromQuery] GetInternalAuditConsolidationReportByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    //Delete Internal Audit Consolidated Report
    [HttpDelete("delete-internal-audit-consolidated-report")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInternalAuditConsolidatedReport([FromBody] DeleteInternalAuditConsolidationReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Internal Audit Consolidated Report
    [HttpGet("search-internal-audit-consolidated-report")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<InternalAuditConsolidationReportModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchInternalAuditConsolidatedReport([FromQuery] SearchInternalAuditConsolidationReportQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}