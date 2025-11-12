using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Recommendation.Create;
using AuditSystem.Application.Features.Recommendation.Delete;
using AuditSystem.Application.Features.Recommendation.Get.GetById;
using AuditSystem.Application.Features.Recommendation.Get.GetByUserId;
using AuditSystem.Application.Features.Recommendation.Search;
using AuditSystem.Application.Features.Recommendation.Update;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.Contract.Common;
using AuditSystem.Contract.Models.Recommendation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Recommendation;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class RecommendationController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Recommendation
    [HttpPost("create-recommendation")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateRecommendationCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRecommendation([FromBody] CreateRecommendationCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Recommendation
    [HttpPut("update-recommendation-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRecommendation([FromBody] UpdateRecommendationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Recommendation
    [HttpGet("get-recommendation-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetRecommendationByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecommendation([FromQuery] GetRecommendationByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);   

    //Get Recommendations By UserId
    [HttpGet("get-recommendations-by-user-id")]
    [ProducesResponseType(typeof(ApiResponse<List<GetRecommendationsByUserIdQueryResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecommendationsByUserId([FromQuery] GetRecommendationsByUserIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Recommendation
    [HttpDelete("delete-recommendation-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRecommendation([FromBody] DeleteRecommendationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Recommendation
    [HttpGet("search-recommendations-by-user-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<RecommendationModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchRecommendation([FromQuery] SearchRecommendationQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}