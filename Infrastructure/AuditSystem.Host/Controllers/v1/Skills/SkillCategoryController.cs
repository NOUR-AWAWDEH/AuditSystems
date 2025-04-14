using AuditSystem.Application.Features.Skills.SkillCategory.Create;
using AuditSystem.Application.Features.Skills.SkillCategory.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Skills;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class SkillCategoryController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Skill Category
    [HttpPost("create-skill-category")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateSkillCategoryCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSkillCategory([FromBody] CreateSkillCategoryCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Skill Category
    [HttpPut("update-skill-category")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSkillCategory([FromBody] UpdateSkillCategoryCommand command) =>
        await ProcessRequestToActionNoContentResultAsync(command);
}