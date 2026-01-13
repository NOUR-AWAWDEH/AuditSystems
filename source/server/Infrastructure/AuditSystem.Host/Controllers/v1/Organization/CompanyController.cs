using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Application.Features.Organization.Company.Create;
using AuditSystem.Application.Features.Organization.Company.Delete;
using AuditSystem.Application.Features.Organization.Company.Get;
using AuditSystem.Application.Features.Organization.Company.Search;
using AuditSystem.Application.Features.Organization.Company.Update;
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
public sealed class CompanyController(IMediator mediator) : ApiControllerBase(mediator)
{
    //Create Company
    [HttpPost("create-company")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CreateCompanyCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyCommand command) =>
        await ProcessRequestToActionResultAsync(command);

    //Update Company
    [HttpPut("update-company")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Get Company
    [HttpGet("get-company")]
    [ProducesResponseType(typeof(ApiResponse<GetCompanyByIdQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompany([FromQuery] GetCompanyByIdQuery query) =>
        await ProcessRequestToActionResultAsync(query);

    //Delete Company
    [HttpDelete("delete-company")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompany([FromBody] DeleteCompanyCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);

    //Search Company
    [HttpGet("search-company")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaginationRequestResponse<CompanyModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchCompany([FromQuery] SearchCompanyQuery query) =>
        await ProcessRequestToActionResultAsync(query);
}