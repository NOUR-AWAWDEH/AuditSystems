using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Risks.ExitControl.Create;
using AuditSystem.Application.Features.Risks.ExitControl.Delete;
using AuditSystem.Application.Features.Risks.ExitControl.Get.GetById;
using AuditSystem.Application.Features.Risks.ExitControl.Get.GetByUserId;
using AuditSystem.Application.Features.Risks.ExitControl.Search;
using AuditSystem.Application.Features.Risks.ExitControl.Update;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.Contract.Common;
using AuditSystem.Contract.Models.Risks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Risks;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class ExitControlController(IMediator mediator) : ApiControllerBase(mediator)
{
    // Create Exit Control
    [HttpPost("create-exit-control")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateExitControlCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateExitControl([FromBody] CreateExitControlCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    // Update Exit Control
    [HttpPut("update-exit-control-by-id")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateExitControl([FromBody] UpdateExitControlCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    // Get Exit Control By Id
    [HttpGet("get-exit-control-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetExitControlByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExitControl([FromQuery] GetExitControlByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    // Get Exit Controls By User Id
    [HttpGet("get-exit-controls-by-user-id")]
    [ProducesResponseType(typeof(ApiResponse<GetExitControlsByUserIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExitControlsByUserId([FromQuery] GetExitControlsByUserIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    // Delete Exit Control
    [HttpDelete("delete-exit-control-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExitControl([FromBody] DeleteExitControlCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    // Search Exit Controls
    [HttpGet("search-exit-controls-by-user-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<ExitControlModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchExitControls([FromQuery] SearchExitControlQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}
