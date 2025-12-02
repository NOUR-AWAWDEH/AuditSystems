using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Organization.JobTitle.Search;
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
    //Search JobTitle
    [HttpGet("search-jobtitles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<JobTitleModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchJobTitle([FromQuery] SearchJobTitleQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}
