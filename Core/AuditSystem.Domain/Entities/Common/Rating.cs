﻿using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities
{
    public class Rating : Entity<Guid>
    {
        public const string High = "High";
        public const string Medium = "Medium";
        public const string Low = "Low";
        public const string Critical = "Critical";
        public const string Minimal = "Minimal";
    }
}
