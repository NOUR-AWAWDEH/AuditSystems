using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Observation.Create;
using AuditSystem.Application.Features.Observation.Delete;
using AuditSystem.Application.Features.Observation.Get.GetById;
using AuditSystem.Application.Features.Observation.Get.GetByUserId;
using AuditSystem.Application.Features.Observation.Search;
using AuditSystem.Application.Features.Observation.Update;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.Contract.Common;
using AuditSystem.Contract.Models.Observation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Observation;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class ObservationController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Observation
    [HttpPost("create-observation")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateObservationCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateObservation([FromBody] CreateObservationCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Observation
    [HttpPut("update-observation-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateObservation([FromBody] UpdateObservationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Observation
    [HttpGet("get-observation-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetObservationByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetObservation([FromQuery] GetObservationByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);   

    //Get Observations By UserId
    [HttpGet("get-observations-by-user-id")]
    [ProducesResponseType(typeof(ApiResponse<List<GetObservationsByUserIdQueryResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetObservationsByUserId([FromQuery] GetObservationsByUserIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Observation
    [HttpDelete("delete-observation-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteObservation([FromBody] DeleteObservationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Observation
    [HttpGet("search-observations-by-user-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<ObservationModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchObservation([FromQuery] SearchObservationQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}