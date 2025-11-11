using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Processes.AuditProcess.Create;
using AuditSystem.Application.Features.Processes.AuditProcess.Delete;
using AuditSystem.Application.Features.Processes.AuditProcess.Get.GetById;
using AuditSystem.Application.Features.Processes.AuditProcess.Get.GetByUserId;
using AuditSystem.Application.Features.Processes.AuditProcess.Search;
using AuditSystem.Application.Features.Processes.AuditProcess.Update;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuditSystem.BusinessLogic.Attributes;

namespace AuditSystem.Host.Controllers.v1.Processes;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class AuditProcessController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Process
    [HttpPost("create-audit-process")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditProcessCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditProcess([FromBody] CreateAuditProcessCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Process
    [HttpPut("update-audit-process")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuditProcess([FromBody] UpdateAuditProcessCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Audit Process By Id
    [HttpGet("get-audit-process-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetAuditProcessByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuditProcess([FromQuery] GetAuditProcessByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Get Audit Process By User Id
    [HttpGet("get-audit-processes-by-user-id")]
    [ProducesResponseType(typeof(ApiResponse<GetAuditProcessesByUserIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuditProcess([FromQuery] GetAuditProcessesByUserIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    //Delete Audit Process
    [HttpDelete("delete-audit-process-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditProcess([FromBody] DeleteAuditProcessCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Audit Process
    [HttpGet("search-audit-processes-by-user-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<AuditProcessModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAuditProcess([FromQuery] SearchAuditProcessQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}