namespace AuditSystem.Contract.Models.Organisation;

public class LocationModel
{
    public required Guid AuditorSettingsId { get; set; }
    public required  string LocationName { get; set; } = string.Empty;
}
