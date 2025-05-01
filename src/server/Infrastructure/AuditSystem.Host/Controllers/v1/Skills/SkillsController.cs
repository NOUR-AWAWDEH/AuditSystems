using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Skills.Skill.Create;
using AuditSystem.Application.Features.Skills.Skill.Delete;
using AuditSystem.Application.Features.Skills.Skill.Get;
using AuditSystem.Application.Features.Skills.Skill.Search;
using AuditSystem.Application.Features.Skills.Skill.Update;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Host.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Skills;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class SkillsController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Skill
    [HttpPost("create-skill")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateSkillCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSkill([FromBody] CreateSkillCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Skill
    [HttpPut("update-skill")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSkill([FromBody] UpdateSkillCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Skill
    [HttpGet("get-skill")]
    [ProducesResponseType(typeof(ApiResponse<GetSkillByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSkill([FromQuery] GetSkillByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    //Delete Skill
    [HttpDelete("delete-skill")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSkill([FromBody] DeleteSkillCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Skill
    [HttpGet("search-skill")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<SkillModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchSkill([FromQuery] SearchSkillQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}