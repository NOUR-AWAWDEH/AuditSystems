using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.RiskControls.RiskProgram.Create;
using AuditSystem.Application.Features.RiskControls.RiskProgram.Delete;
using AuditSystem.Application.Features.RiskControls.RiskProgram.Get;
using AuditSystem.Application.Features.RiskControls.RiskProgram.Search;
using AuditSystem.Application.Features.RiskControls.RiskProgram.Update;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.RiskControls;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class RiskProgramController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Risk Program
    [HttpPost("create-risk-program")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRiskProgramCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRiskProgram([FromBody] CreateRiskProgramCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Risk Program
    [HttpPut("update-risk-program")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRiskProgram([FromBody] UpdateRiskProgramCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Risk Program
    [HttpGet("get-risk-program")]
    [ProducesResponseType(typeof(ApiResponse<GetRiskProgramByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRiskProgram([FromQuery] GetRiskProgramByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    //Delete Risk Program
    [HttpDelete("delete-risk-program")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRiskProgram([FromBody] DeleteRiskProgramCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Risk Program
    [HttpGet("search-risk-program")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<RiskProgramModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchRiskProgram([FromQuery] SearchRiskProgramQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}