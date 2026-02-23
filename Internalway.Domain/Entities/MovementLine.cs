using System;

namespace Internalway.Domain.Entities
{
    public class MovementLine
    {
        public long Id { get; set; }
        public long MovementId { get; set; }
        public long ProductId { get; set; }
        public decimal QuantityDelta { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Movement Movement { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
