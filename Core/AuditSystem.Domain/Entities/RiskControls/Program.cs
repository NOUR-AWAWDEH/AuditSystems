using AuditSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditSystem.Domain.Entities.RiskControls
{
    public class Program : Entity<Guid>
    {
        public Guid RiskControlId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid RatingId { get; set; }
        public string Description { get; set; } = string.Empty;

        public virtual Rating Rating { get; set; } = null!;
        public virtual RiskControl RiskControl { get; set; } = null!;
    }
}
