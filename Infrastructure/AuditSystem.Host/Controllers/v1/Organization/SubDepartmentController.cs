using Ardalis.Result;
using AuditSystem.Application.Features.Organization.SubDepartment.Create;
using AuditSystem.Application.Features.Organization.SubDepartment.Update;
using AuditSystem.Application.Features.Organization.SubDepartment.Delete;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Organization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class SubDepartmentController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Sub Department
    [HttpPost("create-sub-department")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateSubDepartmentCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSubDepartment([FromBody] CreateSubDepartmentCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Sub Department
    [HttpPut("update-sub-department")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSubDepartment([FromBody] UpdateSubDepartmentCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
    
    //Delete Sub Department
    [HttpDelete("delete-sub-department")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSubDepartment([FromBody] DeleteSubDepartmentCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}