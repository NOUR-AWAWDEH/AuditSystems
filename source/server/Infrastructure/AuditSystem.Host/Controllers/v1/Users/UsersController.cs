using AuditSystem.Application.Features.Users.Get.GetActionOwners;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Users;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class UsersController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Get Action Owners
    [HttpGet("get-action-owners")]
    [ProducesResponseType(typeof(ApiResponse<GetActionOwnersQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActionOwners([FromQuery] GetActionOwnersQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}
