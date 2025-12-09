using Ardalis.Result;
using AuditSystem.Application.Features.Users.Create;
using AuditSystem.Application.Features.Users.Delete;
using AuditSystem.Application.Features.Users.Get.GetActionOwners;
using AuditSystem.Application.Features.Users.Get.GetUserById;
using AuditSystem.Application.Features.Users.Get.GetUsersByAdminId;
using AuditSystem.Application.Features.Users.Update;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuditSystem.Domain.Entities.Users;
using AuditSystem.Application.Features.Users.Search;

namespace AuditSystem.Host.Controllers.v1.Users;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Role.Admin)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class UsersController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create User 
    [HttpPost("create-user")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateUserCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Delete User
    [HttpDelete("delete-user-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Action Owners
    [HttpGet("get-action-owners-by-admin-id")]
    [ProducesResponseType(typeof(ApiResponse<GetActionOwnersQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActionOwners([FromQuery] GetActionOwnersQuery query) =>
        await ProcessRequestToActionResultAsync(query);


    //Get User By Id
    [HttpGet("get-user-by-id")]
    [ProducesResponseType(typeof(ApiResponse<GetUserByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById([FromQuery] GetUserByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Get All Users
    [HttpGet("get-users-by-admin-id")]
    [ProducesResponseType(typeof(ApiResponse<GetUsersByAdminIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllUsers([FromQuery] GetUsersByAdminIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Update User
    [HttpPut("update-user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Users by Admin Id
    [HttpGet("search-users-by-admin-id")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchUsersByAdminId([FromQuery] SearchUsersQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}