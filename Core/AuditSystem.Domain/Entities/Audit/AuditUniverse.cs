using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Audit
{
    public class AuditUniverse : Entity<Guid> // every 3 years the audit universe is changed
    {
        public string BusinessObjective { get; set; } = string.Empty;
        public string IndustryUpdate { get; set; } = string.Empty; 
        public string CompanyUpdate { get; set; } = string.Empty;
        public Guid DomainID { get; set; }
        public bool IsFinancialQuantifiable { get; set; } 
        public bool IsSpecialProject { get; set; } = false; // make in Special Project

        public Domain Domain { get; set; } = null!;
    }
}