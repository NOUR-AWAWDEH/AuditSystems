using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Compliance.ComplianceChecklist.Create;
using AuditSystem.Application.Features.Compliance.ComplianceChecklist.Delete;
using AuditSystem.Application.Features.Compliance.ComplianceChecklist.Get;
using AuditSystem.Application.Features.Compliance.ComplianceChecklist.Search;
using AuditSystem.Application.Features.Compliance.ComplianceChecklist.Update;
using AuditSystem.Contract.Models.Compliance;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Compliance;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class ComplianceChecklistController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Compliance Checklist
    [HttpPost("create-compliance")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateComplianceChecklistCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCompliance([FromBody] CreateComplianceChecklistCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Compliance Checklist
    [HttpPut("update-compliance")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCompliance([FromBody] UpdateComplianceChecklistCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Compliance Checklist
    [HttpGet("get-compliance")]
    [ProducesResponseType(typeof(ApiResponse<GetComplianceChecklistByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompliance([FromQuery] GetComplianceChecklistByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Compliance Checklist
    [HttpDelete("delete-compliance")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompliance([FromBody] DeleteComplianceChecklistCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Search Compliance Checklist
    [HttpGet("search-compliance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<ComplianceChecklistModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchCompliance([FromQuery] SearchComplianceChecklistQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}