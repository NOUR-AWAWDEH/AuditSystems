using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Company.Get;

public sealed record class GetCompanyByIdQueryResponse(CompanyModel CompanyModel);