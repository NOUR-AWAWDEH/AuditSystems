using Ardalis.Result;
using AuditSystem.Application.Base;
using System.Windows.Input;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Update;

public sealed record class UpdateReportingFollowUpCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Reporting { get; set; } = string.Empty;
    public required string FollowUp { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}