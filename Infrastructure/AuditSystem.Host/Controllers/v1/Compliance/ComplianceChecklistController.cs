using AuditSystem.Application.Features.Compliance.ComplianceChecklist.Create;
using AuditSystem.Application.Features.Compliance.ComplianceChecklist.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Compliance;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class ComplianceChecklistController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Compliance Checklist
    [HttpPost("create-compliance")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateComplianceChecklistCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCompliance([FromBody] CreateComplianceChecklistCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Compliance Checklist
    [HttpPut("update-compliance")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCompliance([FromBody] UpdateComplianceChecklistCommand command) =>
        await ProcessRequestToActionNoContentResultAsync(command);
}