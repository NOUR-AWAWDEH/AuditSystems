using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.CheckLists
{
    public class Remark : Entity<Guid>
    {
        public Guid CheckListManagementId { get; set; }
        public string RemarkType { get; set; } = string.Empty;

        public CheckListManagement CheckListManagement { get; set; } = null!;
    }
}