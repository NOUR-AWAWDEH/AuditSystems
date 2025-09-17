using AuditSystem.Application.Features.SupportingDocs.Create.Upload;
using AuditSystem.Application.Features.SupportingDocs.Get.GetURLByFileId;
using AuditSystem.Application.Features.SupportingDocs.Get.GetByUserId;
using AuditSystem.Application.Features.SupportingDocs.Search;
using AuditSystem.Contract.Common;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Application.Features.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuditSystem.Application.Features.SupportingDocs.Delete;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuditSystem.BusinessLogic.Attributes;

namespace AuditSystem.Host.Controllers.v1.SupportingDocs;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class SupportingDocsController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Upload Supporting Document
    [HttpPost("upload-supporting-document")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<UploadSupportingDocCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> UploadSupportingDocument([FromForm] UploadSupportingDocCommand command)
        => await ProcessRequestToActionResultAsync(command);

    //Get Supporting Document URL by File ID
    [HttpGet("get-supporting-doc-url-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetSupportingDocURLQueryByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSupportingDocURL([FromQuery] GetSupportingDocURLQueryById query)
        => await ProcessRequestToActionResultAsync(query);

    //Get Supporting Documents by User ID
    [HttpGet("get-supporting-docs-by-user-id")]
    [ProducesResponseType(typeof(ApiResponse<GetSupportingDocsByUserIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSupportingDocsByUserId([FromQuery] GetSupportingDocsByUserIdQuery query)
        => await ProcessRequestToActionResultAsync(query);

    //Search Supporting Documents by User ID
    [HttpGet("search-supporting-docs-by-user-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<SupportingDocModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchSupportingDocsByUserId([FromQuery] SearchSupportingDocsQuery query)
        => await ProcessRequestToActionResultAsync(query);

    //Delete Supporting Document by ID
    [HttpDelete("delete-supporting-doc-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSupportingDoc([FromBody] DeleteSupportingDocCommand command)
        => await ProcessRequestToActionNoContentResultAsync<Result>(command);
}