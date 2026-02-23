using System;
using System.Collections.Generic;

namespace Internalway.Domain.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public long BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Sku { get; set; }
        public string? Description { get; set; }
        public decimal? ListPrice { get; set; }
        public string? Currency { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Brand Brand { get; set; } = null!;
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<MovementLine> MovementLines { get; set; } = new List<MovementLine>();
    }
}
