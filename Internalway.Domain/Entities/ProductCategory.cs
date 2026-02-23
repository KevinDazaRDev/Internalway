using System;

namespace Internalway.Domain.Entities
{
    public class ProductCategory
    {
        public long ProductId { get; set; }
        public long CategoryId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Product Product { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
