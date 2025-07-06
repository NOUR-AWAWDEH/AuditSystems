using Ardalis.Result;
using AuditSystem.Application.Features.Audit.AuditUniverseObjective.Create;
using AuditSystem.Application.Features.Audit.AuditUniverseObjective.Delete;
using AuditSystem.Application.Features.Audit.AuditUniverseObjective.Get;
using AuditSystem.Application.Features.Audit.AuditUniverseObjective.Search;
using AuditSystem.Application.Features.Audit.AuditUniverseObjective.Update;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Common;
using AuditSystem.Contract.Models.Audit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class AuditUniverseObjectiveController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Universe Objective
    [HttpPost("create-audit-universe-objective")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditUniverseObjectiveCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditUniverseObjective([FromBody] CreateAuditUniverseObjectiveCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Universe Objective
    [HttpPut("update-audit-universe-objective")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditUniverseObjective([FromBody] UpdateAuditUniverseObjectiveCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Audit Universe Objective
    [HttpGet("get-audit-universe-objective")]
    [ProducesResponseType(typeof(ApiResponse<GetAuditUniverseObjectiveByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuditUniverseObjective([FromQuery] GetAuditUniverseObjectiveByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Audit Universe Objective
    [HttpDelete("delete-audit-universe-objective")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditUniverseObjective([FromBody] DeleteAuditUniverseObjectiveCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Search Audit Universe Objective
    [HttpGet("search-audit-universe-objective")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<AuditUniverseObjectiveModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAuditUniverseObjective([FromQuery] SearchAuditUniverseObjectiveQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}