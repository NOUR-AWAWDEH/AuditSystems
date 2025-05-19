using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Organization.Department.Create;
using AuditSystem.Application.Features.Organization.Department.Delete;
using AuditSystem.Application.Features.Organization.Department.Get;
using AuditSystem.Application.Features.Organization.Department.Search;
using AuditSystem.Application.Features.Organization.Department.Update;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AuditSystem.Host.Controllers.v1.Organization;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    
    //Get Department
    [HttpGet("get-department")]
    [ProducesResponseType(typeof(ApiResponse<GetDepartmentByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepartment([FromQuery] GetDepartmentByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);
    
    //Delete Department
    [HttpDelete("delete-department")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDepartment([FromBody] DeleteDepartmentCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Department
    [HttpGet("search-department")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<DepartmentModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchDepartment([FromQuery] SearchDepartmentQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}