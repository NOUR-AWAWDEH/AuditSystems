using Ardalis.Result;
using AuditSystem.Application.Features.Organisation.Company.Create;
using AuditSystem.Application.Features.Organisation.Company.Update;
using AuditSystem.Application.Features.Organization.Company.Delete;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Organization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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

    //Delete Company
    [HttpDelete("delete-company")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompany([FromBody] DeleteCompanyCommand command) =>
        await ProcessRequestToActionNoContentResultAsync<Result>(command);
}