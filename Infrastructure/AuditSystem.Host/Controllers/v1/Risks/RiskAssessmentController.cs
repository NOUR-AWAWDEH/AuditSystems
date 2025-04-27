using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Risks.RiskAssessment.Create;
using AuditSystem.Application.Features.Risks.RiskAssessment.Delete;
using AuditSystem.Application.Features.Risks.RiskAssessment.Get;
using AuditSystem.Application.Features.Risks.RiskAssessment.Search;
using AuditSystem.Application.Features.Risks.RiskAssessment.Update;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Risks;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class RiskAssessmentController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Risk Assessment
    [HttpPost("create-risk-assessment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRiskAssessmentCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRiskAssessment([FromBody] CreateRiskAssessmentCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Risk Assessment
    [HttpPut("update-risk-assessment")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRiskAssessment([FromBody] UpdateRiskAssessmentCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Risk Assessment
    [HttpGet("get-risk-assessment")]
    [ProducesResponseType(typeof(ApiResponse<GetRiskAssessmentByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRiskAssessment([FromQuery] GetRiskAssessmentByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    //Delete Risk Assessment
    [HttpDelete("delete-risk-assessment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRiskAssessment([FromBody] DeleteRiskAssessmentCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Risk Assessment
    [HttpGet("search-risk-assessment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<RiskAssessmentModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchRiskAssessment([FromQuery] SearchRiskAssessmentQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}