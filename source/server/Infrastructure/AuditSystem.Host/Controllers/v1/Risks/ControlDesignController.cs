using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Risks.ControlDesign.Create;
using AuditSystem.Application.Features.Risks.ControlDesign.Delete;
using AuditSystem.Application.Features.Risks.ControlDesign.Get.GetById;
using AuditSystem.Application.Features.Risks.ControlDesign.Get.GetByUserId;
using AuditSystem.Application.Features.Risks.ControlDesign.Search;
using AuditSystem.Application.Features.Risks.ControlDesign.Update;
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
public sealed class ControlDesignController(IMediator mediator) : ApiControllerBase(mediator)
{
    // Create Control Design
    [HttpPost("create-control-design")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateControlDesignCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateControlDesign([FromBody] CreateControlDesignCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    // Update Control Design
    [HttpPut("update-control-design-by-id")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateControlDesign([FromBody] UpdateControlDesignCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    // Get Control Design By Id
    [HttpGet("get-control-design-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetControlDesignByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetControlDesign([FromQuery] GetControlDesignByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    // Get Control Designs By User Id
    [HttpGet("get-control-designs-by-user-id")]
    [ProducesResponseType(typeof(ApiResponse<GetControlDesignsByUserIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetControlDesignsByUserId([FromQuery] GetControlDesignsByUserIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    // Delete Control Design
    [HttpDelete("delete-control-design-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteControlDesign([FromBody] DeleteControlDesignCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    // Search Control Designs
    [HttpGet("search-control-designs-by-user-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<ControlDesignModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchControlDesigns([FromQuery] SearchControlDesignQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}
