using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Audit
{
    public class AuditUniverse : Entity<Guid>
    {
        public string BusinessObjectiveText { get; set; } = string.Empty;
        public string IndustryUpdate { get; set; } = string.Empty; 
        public string CompanyUpdate { get; set; } = string.Empty;
        public Guid DomainID { get; set; }
        public bool IsFinancialQuantifiable { get; set; } 
        public bool IsSpecialProject { get; set; } = false;

        public Domain Domain { get; set; } = null!;
    }
}