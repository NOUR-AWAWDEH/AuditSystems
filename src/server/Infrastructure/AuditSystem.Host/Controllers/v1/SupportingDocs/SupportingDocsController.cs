using AuditSystem.Application.Features.SupportingDocs.Get.GetByUserId;
using AuditSystem.Application.Features.SupportingDocs.Get.DownloadByFileId;
using AuditSystem.Application.Features.SupportingDocs.Create.Uplode;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuditSystem.Contract.Models.SupportingDocs;

namespace AuditSystem.Host.Controllers.v1.SupportingDocs;

[Route("api/[controller]")]
[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class SupportingDocsController(IMediator mediator) : ApiControllerBase(mediator)
{

    //Upload Supporting Document
    [HttpPost("upload-supporting-document")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<UploadSupportingDocCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> UploadSupportingDocument([FromForm] UploadSupportingDocCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Get All Supporting Document for the User
    [HttpGet("user/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<List<SupportingDocModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetSupportingDocumentsByUserId(Guid id)
    {
        var query = new GetSupportingDocsByUserIdQuery(id);
        return ProcessRequestToActionResultAsync(query);
    }


    //[HttpGet("download/{id}")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> DownloadFile(Guid id) =>
    //    await ProcessRequestToActionResultAsync(new DownloadSupportingDocQueryById(id));

}