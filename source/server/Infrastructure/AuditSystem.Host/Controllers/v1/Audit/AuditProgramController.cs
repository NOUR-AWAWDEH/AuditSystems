using Ardalis.Result;
using AuditSystem.Application.Features.Audit.AuditProgram.Create;
using AuditSystem.Application.Features.Audit.AuditProgram.Delete;
using AuditSystem.Application.Features.Audit.AuditProgram.Get.GetById;
using AuditSystem.Application.Features.Audit.AuditProgram.Get.GetByUserId;
using AuditSystem.Application.Features.Audit.AuditProgram.Search;
using AuditSystem.Application.Features.Audit.AuditProgram.Update;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.BusinessLogic.Attributes;
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
[AutoInjectUserId]
public sealed class AuditProgramController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Audit Program
    [HttpPost("create-audit-program")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateAuditProgramCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuditProgram([FromBody] CreateAuditProgramCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Audit Program
    [HttpPut("update-audit-program-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAuditProgram([FromBody] UpdateAuditProgramCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Audit Program
    [HttpGet("get-audit-program-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetAuditProgramByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuditProgram([FromQuery] GetAuditProgramByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);   

    //Get Audit Programs By UserId
    [HttpGet("get-audit-programs-by-user-id")]
    [ProducesResponseType(typeof(ApiResponse<List<GetAuditProgramsByUserIdQueryResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuditProgramsByUserId([FromQuery] GetAuditProgramsByUserIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Audit Program
    [HttpDelete("delete-audit-program-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditProgram([FromBody] DeleteAuditProgramCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Audit Program
    [HttpGet("search-audit-programs-by-user-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<AuditProgramModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAuditProgram([FromQuery] SearchAuditProgramQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}