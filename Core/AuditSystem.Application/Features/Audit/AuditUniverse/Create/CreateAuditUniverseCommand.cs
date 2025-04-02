using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Create;

public sealed record class CreateAuditUniverseCommand : ICommand<Result<CreateAuditUniverseCommandResponse>>
{
    public required string BusinessObjective { get; set; } = string.Empty;
    public string IndustryUpdate { get; set; } = string.Empty;
    public string CompanyUpdate { get; set; } = string.Empty;
    public required Guid DomainId { get; set; }
    public bool IsFinancialQuantifiable { get; set; } = false;
    public bool IsSpecialProject { get; set; } = false;
    public Guid SpecialProjectId { get; set; }
}