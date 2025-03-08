using AuditSystem.Domain.Entities.Common;


namespace AuditSystem.Domain.Entities.Users 
{
    public class SkillSet : Entity<Guid>
    {
        public Guid UserManagementId  { get; set; }
        public int SkillId { get; set; } 
        public string ProficiencyLevel { get; set; } = string.Empty;


        public UserManagement UserManagement { get; set; } = null!;
        public Skill Skill { get; set; } = null!;
    }
}