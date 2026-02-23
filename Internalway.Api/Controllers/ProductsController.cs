using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Internalway.Api.Contracts.Products;
using Internalway.Domain.Entities;
using Internalway.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Internalway.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly InternalwayDbContext _db;

        public ProductsController(InternalwayDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken cancellationToken)
        {
            var items = await _db.Products
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new ProductDto(x.Id, x.BrandId, x.Name, x.Slug, x.Sku, x.Description, x.ListPrice, x.Currency, x.IsActive, x.CreatedAt, x.UpdatedAt))
                .ToListAsync(cancellationToken);

            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ProductDto>> GetById(long id, CancellationToken cancellationToken)
        {
            var entity = await _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            return Ok(new ProductDto(entity.Id, entity.BrandId, entity.Name, entity.Slug, entity.Sku, entity.Description, entity.ListPrice, entity.Currency, entity.IsActive, entity.CreatedAt, entity.UpdatedAt));
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
        {
            var brandExists = await _db.Brands.AnyAsync(x => x.Id == request.BrandId, cancellationToken);
            if (!brandExists)
            {
                return BadRequest("Brand not found.");
            }

            var now = DateTimeOffset.UtcNow;
            var entity = new Product
            {
                BrandId = request.BrandId,
                Name = request.Name,
                Slug = request.Slug,
                Sku = request.Sku,
                Description = request.Description,
                ListPrice = request.ListPrice,
                Currency = request.Currency,
                IsActive = request.IsActive,
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.Products.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            var dto = new ProductDto(entity.Id, entity.BrandId, entity.Name, entity.Slug, entity.Sku, entity.Description, entity.ListPrice, entity.Currency, entity.IsActive, entity.CreatedAt, entity.UpdatedAt);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ProductDto>> Update(long id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var brandExists = await _db.Brands.AnyAsync(x => x.Id == request.BrandId, cancellationToken);
            if (!brandExists)
            {
                return BadRequest("Brand not found.");
            }

            var entity = await _db.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            entity.BrandId = request.BrandId;
            entity.Name = request.Name;
            entity.Slug = request.Slug;
            entity.Sku = request.Sku;
            entity.Description = request.Description;
            entity.ListPrice = request.ListPrice;
            entity.Currency = request.Currency;
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);

            var dto = new ProductDto(entity.Id, entity.BrandId, entity.Name, entity.Slug, entity.Sku, entity.Description, entity.ListPrice, entity.Currency, entity.IsActive, entity.CreatedAt, entity.UpdatedAt);
            return Ok(dto);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            var entity = await _db.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            _db.Products.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}

