using Ardalis.Result;
using AuditSystem.Application.Features.Skills.Skill.Create;
using AuditSystem.Application.Features.Skills.Skill.Delete;
using AuditSystem.Application.Features.Skills.Skill.Update;
using AuditSystem.Host.Responses;
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
    
    //Delete Skill
    [HttpDelete("delete-skill")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSkill([FromBody] DeleteSkillCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}