using AuditSystem.Application.Features.Audit.SpecialProject.Create;
using AuditSystem.Application.Features.Audit.SpecialProject.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class SpecialProjectController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Specilal Project
    [HttpPost("create-special-project")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateSpecialProjectCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSpecialProject([FromBody] CreateSpecialProjectCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Special Project
    [HttpPut("update-special-project")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSpecialProject([FromBody] UpdateSpecialProjectCommand command) =>
        await ProcessRequestToActionNoContentResultAsync(command);

}