using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Update;

public sealed record class UpdateAuditUniverseCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string BusinessObjective { get; set; } = string.Empty;
    public string IndustryUpdate { get; set; } = string.Empty;
    public string CompanyUpdate { get; set; } = string.Empty;
    public required Guid DomainId { get; set; }
    public required bool IsFinancialQuantifiable { get; set; } = false;
    public required bool IsSpecialProject { get; set; } = false;
    public Guid SpecialProjectId { get; set; }
}
