using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Internalway.Api.Contracts.Movements;
using Internalway.Domain.Entities;
using Internalway.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Internalway.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/movements")]
    public class MovementsController : ControllerBase
    {
        private readonly InternalwayDbContext _db;

        public MovementsController(InternalwayDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovementDto>>> GetAll(CancellationToken cancellationToken)
        {
            var items = await _db.Movements
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new MovementDto(x.Id, x.Type, x.ClientId, x.Notes, x.OccurredAt, x.CreatedAt, x.UpdatedAt))
                .ToListAsync(cancellationToken);

            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MovementDto>> GetById(long id, CancellationToken cancellationToken)
        {
            var entity = await _db.Movements.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            return Ok(new MovementDto(entity.Id, entity.Type, entity.ClientId, entity.Notes, entity.OccurredAt, entity.CreatedAt, entity.UpdatedAt));
        }

        [HttpPost]
        public async Task<ActionResult<MovementDto>> Create([FromBody] CreateMovementRequest request, CancellationToken cancellationToken)
        {
            if (request.ClientId.HasValue)
            {
                var clientExists = await _db.Clients.AnyAsync(x => x.Id == request.ClientId.Value, cancellationToken);
                if (!clientExists)
                {
                    return BadRequest("Client not found.");
                }
            }

            var now = DateTimeOffset.UtcNow;
            var entity = new Movement
            {
                Type = request.Type,
                ClientId = request.ClientId,
                Notes = request.Notes,
                OccurredAt = request.OccurredAt ?? now,
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.Movements.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            var dto = new MovementDto(entity.Id, entity.Type, entity.ClientId, entity.Notes, entity.OccurredAt, entity.CreatedAt, entity.UpdatedAt);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<MovementDto>> Update(long id, [FromBody] UpdateMovementRequest request, CancellationToken cancellationToken)
        {
            if (request.ClientId.HasValue)
            {
                var clientExists = await _db.Clients.AnyAsync(x => x.Id == request.ClientId.Value, cancellationToken);
                if (!clientExists)
                {
                    return BadRequest("Client not found.");
                }
            }

            var entity = await _db.Movements.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            entity.Type = request.Type;
            entity.ClientId = request.ClientId;
            entity.Notes = request.Notes;
            entity.OccurredAt = request.OccurredAt ?? entity.OccurredAt;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);

            var dto = new MovementDto(entity.Id, entity.Type, entity.ClientId, entity.Notes, entity.OccurredAt, entity.CreatedAt, entity.UpdatedAt);
            return Ok(dto);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            var entity = await _db.Movements.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            _db.Movements.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}

