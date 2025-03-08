using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.SupportingDocs
{
    public class SupportingDoc
    {
        public int AdminSettingsID { get; set; }
        public string FileName { get; set; } = string.Empty;
        public int FileSize {get; set;}
        public string URL {get; set;} = string.Empty;

        public AdminSettings AdminSettings { get; set; } = null!;
    }
}