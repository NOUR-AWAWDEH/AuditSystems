using AuditSystem.Application.Features.Skills.SkillSet.Create;
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
        await ProcessRequestToActionNoContentResultAsync(command);
}