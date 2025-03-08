using AuditSystem.Domain.Entities.Common;
namespace AuditSystem.Domain.Entities.Audit
{
    public class AuditUniverseObjectives : Entity<Guid>
    {
        public Guid AuditUniverseID { get; set; }
        public int SerialNumber { get; set; }
        public string Impact { get; set; } = string.Empty;
        public int  Amount { get; set; }    
        public int ImpactAmount { get; set; }
        public double Percentage { get; set; }

        public AuditUniverse AuditUniverse { get; set; } = null!;
    }
}