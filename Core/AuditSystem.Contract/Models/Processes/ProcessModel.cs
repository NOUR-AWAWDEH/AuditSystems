using AuditSystem.Contract.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditSystem.Contract.Models.Processes
{
    class ProcessModel : BaseModel<Guid>
    {
        public string ProcessName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid RatingId { get; set; }
        public Guid AuditSettingsId { get; set; }
    }
}
