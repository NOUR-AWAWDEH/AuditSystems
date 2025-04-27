using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Risks.Risk.Create;
using AuditSystem.Application.Features.Risks.Risk.Delete;
using AuditSystem.Application.Features.Risks.Risk.Get;
using AuditSystem.Application.Features.Risks.Risk.Search;
using AuditSystem.Application.Features.Risks.Risk.Update;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Risks;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class RiskController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Craete Risk
    [HttpPost("Create-Risk")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRiskCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRisk([FromBody] CreateRiskCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Risk
    [HttpPut("Update-Risk")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRisk([FromBody] UpdateRiskCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Risk
    [HttpGet("Get-Risk")]
    [ProducesResponseType(typeof(ApiResponse<GetRiskByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRisk([FromQuery] GetRiskByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Risk
    [HttpDelete("Delete-Risk")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRisk([FromBody] DeleteRiskCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Risk
    [HttpGet("Search-Risk")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<RiskModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchRisk([FromQuery] SearchRiskQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}