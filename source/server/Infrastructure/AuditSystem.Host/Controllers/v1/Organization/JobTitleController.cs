using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Organization.JobTitle.Create;
using AuditSystem.Application.Features.Organization.JobTitle.Delete;
using AuditSystem.Application.Features.Organization.JobTitle.Get.GetById;
using AuditSystem.Application.Features.Organization.JobTitle.Get.GetByUserId;
using AuditSystem.Application.Features.Organization.JobTitle.Search;
using AuditSystem.Application.Features.Organization.JobTitle.Update;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.Contract.Common;
using AuditSystem.Contract.Models.Organization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Organization;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AutoInjectUserId]
public sealed class JobTitleController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create JobTitle
    [HttpPost("create-jobtitle")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateJobTitleCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateJobTitle([FromBody] CreateJobTitleCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update JobTitle
    [HttpPut("update-jobtitle")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateJobTitle([FromBody] UpdateJobTitleCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get JobTitle By Id
    [HttpGet("get-jobtitle-by-id")]
    [ProducesResponseType(typeof(ApiResponse<List<GetJobTitleByIdQueryResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobTitleById([FromQuery] GetJobTitleByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Get JobTitle By User Id
    [HttpGet("get-jobtitle-by-user-id")]
    [ProducesResponseType(typeof(ApiResponse<List<GetJobTitlesByUserIdQueryResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobTitleByUserId([FromQuery] GetJobTitlesByUserIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete JobTitle
    [HttpDelete("delete-jobtitle-by-id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobTitle([FromBody] DeleteJobTitleCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search JobTitle
    [HttpGet("search-jobtitles-by-user-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<JobTitleModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchJobTitle([FromQuery] SearchJobTitleQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}