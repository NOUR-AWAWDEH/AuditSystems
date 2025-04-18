using Ardalis.Result;
using AuditSystem.Application.Features.Organization.Department.Create;
using AuditSystem.Application.Features.Organization.Department.Update;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Organization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class DepartmentController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Department
    [HttpPost("create-department")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateDepartmentCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Department
    [HttpPut("update-department")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}