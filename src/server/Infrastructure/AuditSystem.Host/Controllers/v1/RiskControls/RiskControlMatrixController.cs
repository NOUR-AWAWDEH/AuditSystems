using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Create;
using AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Delete;
using AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Get;
using AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Search;
using AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Update;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Host.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.RiskControls;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class RiskControlMatrixController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Risk Control Matrix
    [HttpPost("create-risk-control-matrix")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRiskControlMatrixCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRiskControlMatrix([FromBody] CreateRiskControlMatrixCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Risk Control Matrix
    [HttpPut("update-risk-control-matrix")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRiskControlMatrix([FromBody] UpdateRiskControlMatrixCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Risk Control Matrix
    [HttpGet("get-risk-control-matrix")]
    [ProducesResponseType(typeof(ApiResponse<GetRiskControlMatrixByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRiskControlMatrix([FromQuery] GetRiskControlMatrixByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Risk Control Matrix
    [HttpDelete("delete-risk-control-matrix")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRiskControlMatrix([FromBody] DeleteRiskControlMatrixCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Risk Control Matrix
    [HttpGet("search-risk-control-matrix")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<RiskControlMatrixModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchRiskControlMatrix([FromQuery] SearchRiskControlMatrixQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}