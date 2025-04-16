using Ardalis.Result;
using AuditSystem.Application.Features.Skills.SkillSet.Create;
using AuditSystem.Application.Features.Skills.SkillSet.Delete;
using AuditSystem.Application.Features.Skills.SkillSet.Update;
using AuditSystem.Host.Responses;
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
    
    //Delete Skill Set
    [HttpDelete("delete-skill-set")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSkillSet([FromQuery] DeleteSkillSetCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}