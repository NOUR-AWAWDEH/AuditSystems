using Ardalis.Result;
using AuditSystem.Application.Features.Common.Rating.Create;
using AuditSystem.Application.Features.Common.Rating.Update;
using AuditSystem.Host.Responses;
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
}