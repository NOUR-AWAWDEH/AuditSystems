using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Organization.SubDepartment.Create;
using AuditSystem.Application.Features.Organization.SubDepartment.Delete;
using AuditSystem.Application.Features.Organization.SubDepartment.Get;
using AuditSystem.Application.Features.Organization.SubDepartment.Search;
using AuditSystem.Application.Features.Organization.SubDepartment.Update;
using AuditSystem.Contract.Models.Organization;
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

    //Get Sub Department
    [HttpGet("get-sub-department")]
    [ProducesResponseType(typeof(ApiResponse<GetSubDepartmentByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubDepartment([FromQuery] GetSubDepartmentByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Sub Department
    [HttpDelete("delete-sub-department")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSubDepartment([FromBody] DeleteSubDepartmentCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Sub Department
    [HttpGet("search-sub-department")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<SubDepartmentModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchSubDepartment([FromQuery] SearchSubDepartmentQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}