namespace AuditSystem.Contract.Models.Organisation
{
    public class SubLocationModel
    {
        public required Guid LocationId { get; set; }
        public required string Name { get; set; } = string.Empty;
    }
}
