using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Internalway.Api.Contracts.ProductCategories;
using Internalway.Domain.Entities;
using Internalway.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Internalway.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/product-categories")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly InternalwayDbContext _db;

        public ProductCategoriesController(InternalwayDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetAll(CancellationToken cancellationToken)
        {
            var items = await _db.ProductCategories
                .AsNoTracking()
                .OrderBy(x => x.ProductId)
                .ThenBy(x => x.CategoryId)
                .Select(x => new ProductCategoryDto(x.ProductId, x.CategoryId, x.CreatedAt))
                .ToListAsync(cancellationToken);

            return Ok(items);
        }

        [HttpGet("{productId:long}/{categoryId:long}")]
        public async Task<ActionResult<ProductCategoryDto>> GetById(long productId, long categoryId, CancellationToken cancellationToken)
        {
            var entity = await _db.ProductCategories.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.CategoryId == categoryId, cancellationToken);

            if (entity is null)
            {
                return NotFound();
            }

            return Ok(new ProductCategoryDto(entity.ProductId, entity.CategoryId, entity.CreatedAt));
        }

        [HttpPost]
        public async Task<ActionResult<ProductCategoryDto>> Create([FromBody] CreateProductCategoryRequest request, CancellationToken cancellationToken)
        {
            var productExists = await _db.Products.AnyAsync(x => x.Id == request.ProductId, cancellationToken);
            if (!productExists)
            {
                return BadRequest("Product not found.");
            }

            var categoryExists = await _db.Categories.AnyAsync(x => x.Id == request.CategoryId, cancellationToken);
            if (!categoryExists)
            {
                return BadRequest("Category not found.");
            }

            var exists = await _db.ProductCategories.AnyAsync(x => x.ProductId == request.ProductId && x.CategoryId == request.CategoryId, cancellationToken);
            if (exists)
            {
                return Conflict("Product category already exists.");
            }

            var entity = new ProductCategory
            {
                ProductId = request.ProductId,
                CategoryId = request.CategoryId,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _db.ProductCategories.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            var dto = new ProductCategoryDto(entity.ProductId, entity.CategoryId, entity.CreatedAt);
            return CreatedAtAction(nameof(GetById), new { productId = entity.ProductId, categoryId = entity.CategoryId }, dto);
        }

        [HttpDelete("{productId:long}/{categoryId:long}")]
        public async Task<IActionResult> Delete(long productId, long categoryId, CancellationToken cancellationToken)
        {
            var entity = await _db.ProductCategories.FirstOrDefaultAsync(x => x.ProductId == productId && x.CategoryId == categoryId, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            _db.ProductCategories.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}

