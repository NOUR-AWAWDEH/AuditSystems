using Ardalis.Result;
using AuditSystem.Application.Features.RiskControls.Objective.Create;
using AuditSystem.Application.Features.RiskControls.Objective.Delete;
using AuditSystem.Application.Features.RiskControls.Objective.Get.GetById;
using AuditSystem.Application.Features.RiskControls.Objective.Get.GetByUserId;
using AuditSystem.Application.Features.RiskControls.Objective.Search;
using AuditSystem.Application.Features.RiskControls.Objective.Update;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Common;
using AuditSystem.Contract.Models.RiskControls;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuditSystem.BusinessLogic.Attributes;

namespace AuditSystem.Host.Controllers.v1.RiskControls;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class ObjectiveController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Objective
    [HttpPost("create-objective")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateObjectiveCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateObjective([FromBody] CreateObjectiveCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Objective
    [HttpPut("update-objective")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateObjective([FromBody] UpdateObjectiveCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Objective By Id
    [HttpGet("get-objective-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetObjectiveByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetObjective([FromQuery] GetObjectiveByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Get Objective By Id User
    [HttpGet("get-objective-by-id-user")]
    [ProducesResponseType(typeof(ApiResponse<GetObjectivesByUserIdQueryResponse>), StatusCodes.Status200OK)]

    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetObjective([FromQuery] GetObjectivesByUserIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Objective
    [HttpDelete("delete-objective-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteObjective([FromQuery] DeleteObjectiveCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Search Objective
    [HttpGet("search-objective-by-user-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<ObjectiveModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchObjective([FromQuery] SearchObjectiveQuery query) =>
        await ProcessRequestToActionResultAsync(query);  

}