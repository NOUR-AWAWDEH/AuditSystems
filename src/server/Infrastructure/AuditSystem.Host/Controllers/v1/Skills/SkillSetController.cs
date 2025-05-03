using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Skills.SkillSet.Create;
using AuditSystem.Application.Features.Skills.SkillSet.Delete;
using AuditSystem.Application.Features.Skills.SkillSet.Get;
using AuditSystem.Application.Features.Skills.SkillSet.Search;
using AuditSystem.Application.Features.Skills.SkillSet.Update;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Skills;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class SkillSetController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Skill Set
    [HttpPost("create-skill-set")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateSkillSetCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSkillSet([FromBody] CreateSkillSetCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Skill Set
    [HttpPut("update-skill-set")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSkillSet([FromBody] UpdateSkillSetCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Skill Set
    [HttpGet("get-skill-set")]
    [ProducesResponseType(typeof(ApiResponse<GetSkillSetByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSkillSet([FromQuery] GetSkillSetByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    //Delete Skill Set
    [HttpDelete("delete-skill-set")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSkillSet([FromQuery] DeleteSkillSetCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Skill Set
    [HttpGet("search-skill-set")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<SkillSetModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchSkillSet([FromQuery] SearchSkillSetQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}