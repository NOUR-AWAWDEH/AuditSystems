using AuditSystem.Application.Features.SupportingDocs.Uplode;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.SupportingDocs;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class SupportingDocsController(IMediator mediator) : ApiControllerBase(mediator)
{

    //Upload Supporting Document
    [HttpPost("upload-supporting-document")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<UploadSupportingDocCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> UploadSupportingDocument([FromForm] UploadSupportingDocCommand command) =>
        await ProcessRequestToActionResultAsync(command);
}