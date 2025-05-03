using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Skills.SkillCategory.Create;
using AuditSystem.Application.Features.Skills.SkillCategory.Delete;
using AuditSystem.Application.Features.Skills.SkillCategory.Get;
using AuditSystem.Application.Features.Skills.SkillCategory.Search;
using AuditSystem.Application.Features.Skills.SkillCategory.Update;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Contract.Common;
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
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Skill Category
    [HttpGet("get-skill-category")]
    [ProducesResponseType(typeof(ApiResponse<GetSkillCategoryByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSkillCategory([FromQuery] GetSkillCategoryByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    //Delete Skill Category
    [HttpDelete("delete-skill-category")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSkillCategory([FromBody] DeleteSkillCategoryCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Skill Category
    [HttpGet("search-skill-category")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<SkillCategoryModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchSkillCategory([FromQuery] SearchSkillCategoryQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}