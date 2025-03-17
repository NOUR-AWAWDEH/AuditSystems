using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Processes;

public class SubProcess : Entity<Guid>
{
    public Guid ProcessId { get; set; }
    public string Particular { get; set; } = string.Empty;

    public virtual AuditProcess Process { get; set; } = null!;
}