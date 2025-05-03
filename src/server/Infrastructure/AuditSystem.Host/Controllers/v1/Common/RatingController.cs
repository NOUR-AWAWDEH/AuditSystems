using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Common.Rating.Create;
using AuditSystem.Application.Features.Common.Rating.Delete;
using AuditSystem.Application.Features.Common.Rating.Get;
using AuditSystem.Application.Features.Common.Rating.Search;
using AuditSystem.Application.Features.Common.Rating.Update;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Common;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class RatingController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Rating
    [HttpPost("create-rating")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRatingCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRating([FromBody] CreateRatingCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Rating
    [HttpPut("update-rating")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRating([FromBody] UpdateRatingCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Rating
    [HttpGet("get-rating")]
    [ProducesResponseType(typeof(ApiResponse<GetRatingByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRating([FromQuery] GetRatingByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Rating
    [HttpDelete("delete-rating")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRating([FromBody] DeleteRatingCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Search Rating
    [HttpGet("search-rating")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<RatingModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchRating([FromQuery] SearchRatingQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}