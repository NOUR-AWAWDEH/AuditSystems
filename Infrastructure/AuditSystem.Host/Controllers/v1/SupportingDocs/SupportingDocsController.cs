using Ardalis.Result;
using AuditSystem.Application.Features.SupportingDocs.Create;
using AuditSystem.Application.Features.SupportingDocs.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.SupportingDocs;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class SupportingDocsController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Supporting Document
    [HttpPost("create-supporting-document")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateSupportingDocCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSupportingDocument([FromBody] CreateSupportingDocCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Supporting Document
    [HttpPut("update-supporting-document")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSupportingDocument([FromBody] UpdateSupportingDocCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}