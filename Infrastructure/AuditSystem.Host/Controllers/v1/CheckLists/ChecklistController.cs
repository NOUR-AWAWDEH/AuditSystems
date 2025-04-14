using AuditSystem.Application.Features.Checklists.Checklist.Create;
using AuditSystem.Application.Features.Checklists.Checklist.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.CheckLists;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class ChecklistController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Checklist
    [HttpPost("create-checklist")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateChecklistCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateChecklist([FromBody] CreateChecklistCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Checklist
    [HttpPut("update-checklist")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateChecklist([FromBody] UpdateChecklistCommand command) =>
        await ProcessRequestToActionNoContentResultAsync(command);
}