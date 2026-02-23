using Internalway.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Internalway.Infrastructure.Persistence
{
    public class InternalwayDbContext : DbContext
    {
        public InternalwayDbContext(DbContextOptions<InternalwayDbContext> options)
            : base(options)
        {
        }

        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Movement> Movements => Set<Movement>();
        public DbSet<MovementLine> MovementLines => Set<MovementLine>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InternalwayDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
