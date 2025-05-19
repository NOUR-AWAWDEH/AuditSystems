using Ardalis.Result;
using AuditSystem.Application.Features.Organization.Location.Create;
using AuditSystem.Application.Features.Organization.Location.Update;
using AuditSystem.Application.Features.Organization.Location.Delete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuditSystem.Application.Features.Organization.Location.Get;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Organization.Location.Search;
using AuditSystem.Contract.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AuditSystem.Host.Controllers.v1.Organization;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class LocationController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Location
    [HttpPost("create-location")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateLocationCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateLocation([FromBody] CreateLocationCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Location
    [HttpPut("update-location")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLocation([FromBody] UpdateLocationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Get Location
    [HttpGet("get-location")]
    [ProducesResponseType(typeof(ApiResponse<GetLocationByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLocation([FromQuery] GetLocationByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    //Delete Location
    [HttpDelete("delete-location")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLocation([FromBody] DeleteLocationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Location
    [HttpGet("search-location")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<LocationModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchLocation([FromQuery] SearchLocationQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}