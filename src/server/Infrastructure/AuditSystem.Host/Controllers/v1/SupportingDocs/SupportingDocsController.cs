using AuditSystem.Application.Features.SupportingDocs.Create.Upload;
using AuditSystem.Application.Features.SupportingDocs.Get.GetURLByFileId;
using AuditSystem.Application.Features.SupportingDocs.Get.GetByUserId;
using AuditSystem.Contract.Common;
using AuditSystem.Contract.Models.SupportingDocs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuditSystem.Application.Features.SupportingDocs.Delete;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AuditSystem.Host.Controllers.v1.SupportingDocs;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

    //Download Supporting Document
    [HttpGet("download/{fileId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetSupportingDocURLQueryByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> DownloadSupportingDocument(Guid fileId)
    {
        var query = new GetSupportingDocURLQueryById(fileId);
        return ProcessRequestToActionResultAsync(query);
    }

    //Delete Supporting Document
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteSupportingDocument
    ([FromBody]DeleteSupportingDocCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}