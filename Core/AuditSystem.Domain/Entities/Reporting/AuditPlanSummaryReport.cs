using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Reporting
{
    public class AuditPlanSummaryReport
    {
        public int SerialNumber { get; set; }
        public string ReportName { get; set; } = string.Empty;
        public DateOnly ReportDate { get; set; }
        public Guid CreatedByID { get; set; } 
        public string Status { get; set; } = string.Empty;

        public User Creator { get; set; } = null!;
    }
}