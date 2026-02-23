using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Internalway.Api.Contracts.Brands;
using Internalway.Domain.Entities;
using Internalway.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Internalway.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/brands")]
    public class BrandsController : ControllerBase
    {
        private readonly InternalwayDbContext _db;

        public BrandsController(InternalwayDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAll(CancellationToken cancellationToken)
        {
            var items = await _db.Brands
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new BrandDto(x.Id, x.Name, x.Slug, x.Description, x.CreatedAt, x.UpdatedAt))
                .ToListAsync(cancellationToken);

            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<BrandDto>> GetById(long id, CancellationToken cancellationToken)
        {
            var entity = await _db.Brands.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            return Ok(new BrandDto(entity.Id, entity.Name, entity.Slug, entity.Description, entity.CreatedAt, entity.UpdatedAt));
        }

        [HttpPost]
        public async Task<ActionResult<BrandDto>> Create([FromBody] CreateBrandRequest request, CancellationToken cancellationToken)
        {
            var now = DateTimeOffset.UtcNow;
            var entity = new Brand
            {
                Name = request.Name,
                Slug = request.Slug,
                Description = request.Description,
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.Brands.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            var dto = new BrandDto(entity.Id, entity.Name, entity.Slug, entity.Description, entity.CreatedAt, entity.UpdatedAt);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<BrandDto>> Update(long id, [FromBody] UpdateBrandRequest request, CancellationToken cancellationToken)
        {
            var entity = await _db.Brands.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            entity.Name = request.Name;
            entity.Slug = request.Slug;
            entity.Description = request.Description;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);

            var dto = new BrandDto(entity.Id, entity.Name, entity.Slug, entity.Description, entity.CreatedAt, entity.UpdatedAt);
            return Ok(dto);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            var entity = await _db.Brands.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            _db.Brands.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}

