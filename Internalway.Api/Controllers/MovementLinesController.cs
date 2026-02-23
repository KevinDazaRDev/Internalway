using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Internalway.Api.Contracts.MovementLines;
using Internalway.Domain.Entities;
using Internalway.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Internalway.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/movement-lines")]
    public class MovementLinesController : ControllerBase
    {
        private readonly InternalwayDbContext _db;

        public MovementLinesController(InternalwayDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovementLineDto>>> GetAll(CancellationToken cancellationToken)
        {
            var items = await _db.MovementLines
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new MovementLineDto(x.Id, x.MovementId, x.ProductId, x.QuantityDelta, x.UnitPrice, x.CreatedAt))
                .ToListAsync(cancellationToken);

            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MovementLineDto>> GetById(long id, CancellationToken cancellationToken)
        {
            var entity = await _db.MovementLines.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            return Ok(new MovementLineDto(entity.Id, entity.MovementId, entity.ProductId, entity.QuantityDelta, entity.UnitPrice, entity.CreatedAt));
        }

        [HttpPost]
        public async Task<ActionResult<MovementLineDto>> Create([FromBody] CreateMovementLineRequest request, CancellationToken cancellationToken)
        {
            var movementExists = await _db.Movements.AnyAsync(x => x.Id == request.MovementId, cancellationToken);
            if (!movementExists)
            {
                return BadRequest("Movement not found.");
            }

            var productExists = await _db.Products.AnyAsync(x => x.Id == request.ProductId, cancellationToken);
            if (!productExists)
            {
                return BadRequest("Product not found.");
            }

            var entity = new MovementLine
            {
                MovementId = request.MovementId,
                ProductId = request.ProductId,
                QuantityDelta = request.QuantityDelta,
                UnitPrice = request.UnitPrice,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _db.MovementLines.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            var dto = new MovementLineDto(entity.Id, entity.MovementId, entity.ProductId, entity.QuantityDelta, entity.UnitPrice, entity.CreatedAt);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<MovementLineDto>> Update(long id, [FromBody] UpdateMovementLineRequest request, CancellationToken cancellationToken)
        {
            var entity = await _db.MovementLines.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            entity.QuantityDelta = request.QuantityDelta;
            entity.UnitPrice = request.UnitPrice;

            await _db.SaveChangesAsync(cancellationToken);

            var dto = new MovementLineDto(entity.Id, entity.MovementId, entity.ProductId, entity.QuantityDelta, entity.UnitPrice, entity.CreatedAt);
            return Ok(dto);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            var entity = await _db.MovementLines.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            _db.MovementLines.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}

