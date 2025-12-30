using AuditSystem.Domain.Entities.Common;
using System.Text.Json;

namespace AuditSystem.Domain.Entities.Reports;

public class DriftReport : AuditableEntity<Guid>
{
    public string FileUrl { get; set; } = null!;
    public string Status { get; set; } = "Completed";
}