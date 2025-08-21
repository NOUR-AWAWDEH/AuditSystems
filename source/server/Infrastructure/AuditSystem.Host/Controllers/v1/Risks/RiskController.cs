using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Risks.Risk.Create;
using AuditSystem.Application.Features.Risks.Risk.Delete;
using AuditSystem.Application.Features.Risks.Risk.Get.GetById;
using AuditSystem.Application.Features.Risks.Risk.Get.GetByUserId;
using AuditSystem.Application.Features.Risks.Risk.Search;
using AuditSystem.Application.Features.Risks.Risk.Update;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.Contract.Common;
using AuditSystem.Contract.Models.Risks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Risks;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class RiskController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Craete Risk
    [HttpPost("create-risk")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRiskCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRisk([FromBody] CreateRiskCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Risk
    [HttpPut("update-risk")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRisk([FromBody] UpdateRiskCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Risk
    [HttpGet("get-risk-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetRiskByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRisk([FromQuery] GetRiskByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Get Risk By User Id
    [HttpGet("get-risks-by-user-id")]
    [ProducesResponseType(typeof(ApiResponse<GetRisksByUserIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRisk([FromQuery] GetRisksByUserIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Risk
    [HttpDelete("delete-risk-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRisk([FromBody] DeleteRiskCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Risk
    [HttpGet("search-risks-by-user-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<RiskModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchRisk([FromQuery] SearchRiskQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}