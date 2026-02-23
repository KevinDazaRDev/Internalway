using Internalway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internalway.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Slug).IsRequired();
            builder.Property(x => x.Sku);
            builder.Property(x => x.Description);
            builder.Property(x => x.ListPrice).HasPrecision(12, 2);
            builder.Property(x => x.Currency).HasMaxLength(3);
            builder.Property(x => x.IsActive).IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder.HasIndex(x => x.BrandId);
            builder.HasIndex(x => x.Sku);
            builder.HasIndex(x => x.Slug).IsUnique();

            builder.HasMany(x => x.ProductCategories)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
