using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.SupportingDocs.Create;
using AuditSystem.Application.Features.SupportingDocs.Delete;
using AuditSystem.Application.Features.SupportingDocs.Get;
using AuditSystem.Application.Features.SupportingDocs.Search;
using AuditSystem.Application.Features.SupportingDocs.Update;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Host.Repositories;
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

    //Get Supporting Document
    [HttpGet("get-supporting-document")]
    [ProducesResponseType(typeof(ApiResponse<GetSupportingDocsByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSupportingDocument([FromQuery] GetSupportingDocsByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Supporting Document
    [HttpDelete("delete-supporting-document")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSupportingDocument([FromBody] DeleteSupportingDocCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Supporting Document
    [HttpGet("search-supporting-document")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<SupportingDocModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchSupportingDocument([FromQuery] SearchSupportingDocsQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}