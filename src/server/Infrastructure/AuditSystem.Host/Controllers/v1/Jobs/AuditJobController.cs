using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Jobs.AuditJob.Create;
using AuditSystem.Application.Features.Jobs.AuditJob.Delete;
using AuditSystem.Application.Features.Jobs.AuditJob.Get;
using AuditSystem.Application.Features.Jobs.AuditJob.Search;
using AuditSystem.Application.Features.Jobs.AuditJob.Update;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Host.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Jobs;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class AuditJobController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Job
    [HttpPost("create-audit-job")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditJobCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditJob([FromBody] CreateAuditJobCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Job
    [HttpPut("update-audit-job")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditJob([FromBody] UpdateAuditJobCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Audit Job
    [HttpGet("get-audit-job")]
    [ProducesResponseType(typeof(ApiResponse<GetAuditJobByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuditJob([FromQuery] GetAuditJobByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    //Delete Audit Job
    [HttpDelete("delete-audit-job")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditJob([FromBody] DeleteAuditJobCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Search Audit Job
    [HttpGet("search-audit-job")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<AuditJobModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAuditJob([FromQuery] SearchAuditJobQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}