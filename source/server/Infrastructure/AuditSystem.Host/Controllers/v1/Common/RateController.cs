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
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AuditSystem.Host.Controllers.v1.Common;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class RateController(IMediator mediator) : ApiControllerBase(mediator)
{
    // Create Rate
    [HttpPost("create-rate")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRateCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRate([FromBody] CreateRateCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    // Update Rate
    [HttpPut("update-rate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRate([FromBody] UpdateRateCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    // Get Rate by ID
    [HttpGet("get-rate-by-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<GetRateByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRate([FromQuery] GetRateByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    // Delete Rate
    [HttpDelete("delete-rate-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRate([FromBody] DeleteRateCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    // Search Rates
    [HttpGet("search-rates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<RateModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchRates([FromQuery] SearchRatesQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}
