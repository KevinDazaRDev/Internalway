using Internalway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internalway.Infrastructure.Persistence.Configurations
{
    public class MovementLineConfiguration : IEntityTypeConfiguration<MovementLine>
    {
        public void Configure(EntityTypeBuilder<MovementLine> builder)
        {
            builder.ToTable("movement_lines");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.QuantityDelta).HasPrecision(14, 4);
            builder.Property(x => x.UnitPrice).HasPrecision(12, 2);
            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasIndex(x => x.MovementId);
            builder.HasIndex(x => x.ProductId);
            builder.HasIndex(x => new { x.ProductId, x.CreatedAt });

            builder.HasOne(x => x.Movement)
                .WithMany(x => x.Lines)
                .HasForeignKey(x => x.MovementId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.MovementLines)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
