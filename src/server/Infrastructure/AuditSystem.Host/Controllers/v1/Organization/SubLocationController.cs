using Ardalis.Result;
using AuditSystem.Application.Features.Organization.SubLocation.Create;
using AuditSystem.Application.Features.Organization.SubLocation.Update;
using AuditSystem.Application.Features.Organization.SubLocation.Delete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuditSystem.Application.Features.Organization.SubLocation.Get;
using AuditSystem.Application.Features.Organization.SubLocation.Search;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuditSystem.BusinessLogic.Attributes;

namespace AuditSystem.Host.Controllers.v1.Organization;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class SubLocationController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create SubLocation
    [HttpPost("create-sub-location")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateSubLocationCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSubLocation([FromBody] CreateSubLocationCommand command)
        => await ProcessRequestToActionResultAsync(command);

    //Update Sub Location
    [HttpPut("update-sub-location")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSubLocation([FromBody] UpdateSubLocationCommand command)
        => await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Sub Location
    [HttpGet("get-sub-location-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetSubLocationByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubLocation([FromQuery] GetSubLocationByIdQuery query)
        => await ProcessRequestToActionResultAsync(query);

    //Get Sub-Locations by User ID
    [HttpGet("get-sub-locations-by-user-id")]
    [ProducesResponseType(typeof(ApiResponse<GetSubLocationsByUserIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSubLocationsByUserId([FromQuery] GetSubLocationsByUserIdQuery query)
        => await ProcessRequestToActionResultAsync(query);

    //Delete Sub-Location by ID
    [HttpDelete("delete-sub-location-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSubLocation([FromBody] DeleteSubLocationCommand command)
        => await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Sub Location
    [HttpGet("search-sub-location-by-user-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<SubLocationModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchSubLocation([FromQuery] SearchSubLocationQuery query)
        => await ProcessRequestToActionResultAsync(query);
}