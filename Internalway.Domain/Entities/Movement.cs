using System;
using System.Collections.Generic;
using Internalway.Domain.Enums;

namespace Internalway.Domain.Entities
{
    public class Movement
    {
        public long Id { get; set; }
        public MovementType Type { get; set; }
        public long? ClientId { get; set; }
        public string? Notes { get; set; }
        public DateTimeOffset OccurredAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Client? Client { get; set; }
        public ICollection<MovementLine> Lines { get; set; } = new List<MovementLine>();
    }
}
