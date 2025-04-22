using AuditSystem.Contract.Models.UserDesignation;

namespace AuditSystem.Application.Features.UserDesignation.Get;

public sealed record class GetUserDesignationByIdQueryResponse(UserDesignationModel UserDesignationModel);