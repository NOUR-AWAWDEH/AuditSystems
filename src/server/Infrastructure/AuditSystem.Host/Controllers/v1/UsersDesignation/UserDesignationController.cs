using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.UserDesignation.Create;
using AuditSystem.Application.Features.UserDesignation.Delete;
using AuditSystem.Application.Features.UserDesignation.Get;
using AuditSystem.Application.Features.UserDesignation.Search;
using AuditSystem.Application.Features.UserDesignation.Update;
using AuditSystem.Contract.Models.UserDesignation;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.UsersDesignation;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class UserDesignationController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create User Designation
    [HttpPost("create-user-designation")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateUserDesignationCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUserDesignation([FromBody] CreateUserDesignationCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update User Designation
    [HttpPut("update-user-designation")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserDesignation([FromBody] UpdateUserDesignationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get User Designation
    [HttpGet("get-user-designation")]
    [ProducesResponseType(typeof(ApiResponse<GetUserDesignationByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserDesignation([FromQuery] GetUserDesignationByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete User Designation
    [HttpDelete("delete-user-designation")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserDesignation([FromBody] DeleteUserDesignationCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search User Designation
    [HttpGet("search-user-designation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<UserDesignationModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchUserDesignation([FromQuery] SearchUserDesignationQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}