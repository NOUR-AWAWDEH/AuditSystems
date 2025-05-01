using Ardalis.Result;
using AuditSystem.Application.Features.Audit.BusinessObjective.Create;
using AuditSystem.Application.Features.Audit.BusinessObjective.Delete;
using AuditSystem.Application.Features.Audit.BusinessObjective.Get;
using AuditSystem.Application.Features.Audit.BusinessObjective.Search;
using AuditSystem.Application.Features.Audit.BusinessObjective.Update;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Host.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class BusinessObjectiveController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Business Objective
    [HttpPost("create-business-objective")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateBusinessObjectiveCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBusinessObjective([FromBody] CreateBusinessObjectiveCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Business Objective
    [HttpPut("update-business-objective")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBusinessObjective([FromBody] UpdateBusinessObjectiveCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Business Objective
    [HttpGet("get-business-objective")]
    [ProducesResponseType(typeof(ApiResponse<GetBusinessObjectiveByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBusinessObjective([FromQuery] GetBusinessObjectiveByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Business Objective
    [HttpDelete("delete-business-objective")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBusinessObjective([FromBody] DeleteBusinessObjectiveCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Search Business Objective
    [HttpGet("search-business-objective")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<BusinessObjectiveModel>>),StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchBusinessObjective([FromQuery] SearchBusinessObjectiveQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}