namespace AuditSystem.Contract.Models.Organization;

public sealed record JobTitleModel(
    Guid Id,
    string Name,
    Guid CompanyId);
