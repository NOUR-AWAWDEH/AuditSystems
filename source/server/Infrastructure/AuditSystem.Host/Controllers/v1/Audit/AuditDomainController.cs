using Ardalis.Result;
using AuditSystem.Application.Features.Audit.AuditDomain.Create;
using AuditSystem.Application.Features.Audit.AuditDomain.Delete;
using AuditSystem.Application.Features.Audit.AuditDomain.Get;
using AuditSystem.Application.Features.Audit.AuditDomain.Search;
using AuditSystem.Application.Features.Audit.AuditDomain.Update;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Common;
using AuditSystem.Contract.Models.Audit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Audit;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class AuditDomainController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Domain
    [HttpPost("create-audit-domain")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditDomainCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditDomain([FromBody] CreateAuditDomainCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Domain
    [HttpPut("update-audit-domain")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditDomain([FromBody] UpdateAuditDomainCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Audit Domain By Id
    [HttpGet("get-audit-domain-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetAuditDomainByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuditDomainById([FromQuery] GetAuditDomainByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Audit Domain
    [HttpDelete("delete-audit-domain")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditDomain([FromBody] DeleteAuditDomainCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Audit Domain
    [HttpGet("search-audit-domain")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<AuditDomainModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAuditDomain([FromQuery] SearchAuditDomainQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}