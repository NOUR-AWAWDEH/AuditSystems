using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Reports.PlanningReport.Create;
using AuditSystem.Application.Features.Reports.PlanningReport.Delete;
using AuditSystem.Application.Features.Reports.PlanningReport.Get;
using AuditSystem.Application.Features.Reports.PlanningReport.Search;
using AuditSystem.Application.Features.Reports.PlanningReport.Update;
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
public sealed class PlanningReportController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Planning Report
    [HttpPost("create-planning-report")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreatePlanningReportCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePlanningReport([FromBody] CreatePlanningReportCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Planning Report
    [HttpPut("update-planning-report")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePlanningReport([FromBody] UpdatePlanningReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Planning Report
    [HttpGet("get-planning-report")]
    [ProducesResponseType(typeof(ApiResponse<GetPlanningReportByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlanningReport([FromQuery] GetPlanningReportByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Planning Report
    [HttpDelete("delete-planning-report")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePlanningReport([FromBody] DeletePlanningReportCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Planning Report
    [HttpGet("search-planning-report")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<PlanningReportModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchPlanningReport([FromQuery] SearchPlanningReportQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}