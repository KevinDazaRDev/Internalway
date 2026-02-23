using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Internalway.Api.Contracts.Categories;
using Internalway.Domain.Entities;
using Internalway.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Internalway.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly InternalwayDbContext _db;

        public CategoriesController(InternalwayDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken cancellationToken)
        {
            var items = await _db.Categories
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new CategoryDto(x.Id, x.ParentId, x.Name, x.Slug, x.Description, x.CreatedAt, x.UpdatedAt))
                .ToListAsync(cancellationToken);

            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<CategoryDto>> GetById(long id, CancellationToken cancellationToken)
        {
            var entity = await _db.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            return Ok(new CategoryDto(entity.Id, entity.ParentId, entity.Name, entity.Slug, entity.Description, entity.CreatedAt, entity.UpdatedAt));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            if (request.ParentId.HasValue)
            {
                var exists = await _db.Categories.AnyAsync(x => x.Id == request.ParentId.Value, cancellationToken);
                if (!exists)
                {
                    return BadRequest("Parent category not found.");
                }
            }

            var now = DateTimeOffset.UtcNow;
            var entity = new Category
            {
                ParentId = request.ParentId,
                Name = request.Name,
                Slug = request.Slug,
                Description = request.Description,
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.Categories.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            var dto = new CategoryDto(entity.Id, entity.ParentId, entity.Name, entity.Slug, entity.Description, entity.CreatedAt, entity.UpdatedAt);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<CategoryDto>> Update(long id, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            if (request.ParentId.HasValue)
            {
                var exists = await _db.Categories.AnyAsync(x => x.Id == request.ParentId.Value, cancellationToken);
                if (!exists)
                {
                    return BadRequest("Parent category not found.");
                }
            }

            var entity = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            entity.ParentId = request.ParentId;
            entity.Name = request.Name;
            entity.Slug = request.Slug;
            entity.Description = request.Description;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);

            var dto = new CategoryDto(entity.Id, entity.ParentId, entity.Name, entity.Slug, entity.Description, entity.CreatedAt, entity.UpdatedAt);
            return Ok(dto);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            var entity = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            _db.Categories.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}

