using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Recommendation;

public class RecommendationModel : BaseModel<Guid>
{
    public required string Title { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public required Guid ObservationId { get; set; }
}