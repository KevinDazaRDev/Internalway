using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Internalway.Api.Contracts.Clients;
using Internalway.Domain.Entities;
using Internalway.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Internalway.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly InternalwayDbContext _db;

        public ClientsController(InternalwayDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetAll(CancellationToken cancellationToken)
        {
            var items = await _db.Clients
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new ClientDto(x.Id, x.FirstName, x.LastName, x.Email, x.Phone, x.DocumentType, x.DocumentNumber, x.CreatedAt, x.UpdatedAt))
                .ToListAsync(cancellationToken);

            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ClientDto>> GetById(long id, CancellationToken cancellationToken)
        {
            var entity = await _db.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            return Ok(new ClientDto(entity.Id, entity.FirstName, entity.LastName, entity.Email, entity.Phone, entity.DocumentType, entity.DocumentNumber, entity.CreatedAt, entity.UpdatedAt));
        }

        [HttpPost]
        public async Task<ActionResult<ClientDto>> Create([FromBody] CreateClientRequest request, CancellationToken cancellationToken)
        {
            var now = DateTimeOffset.UtcNow;
            var entity = new Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                DocumentType = request.DocumentType,
                DocumentNumber = request.DocumentNumber,
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.Clients.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            var dto = new ClientDto(entity.Id, entity.FirstName, entity.LastName, entity.Email, entity.Phone, entity.DocumentType, entity.DocumentNumber, entity.CreatedAt, entity.UpdatedAt);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ClientDto>> Update(long id, [FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var entity = await _db.Clients.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.Email = request.Email;
            entity.Phone = request.Phone;
            entity.DocumentType = request.DocumentType;
            entity.DocumentNumber = request.DocumentNumber;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);

            var dto = new ClientDto(entity.Id, entity.FirstName, entity.LastName, entity.Email, entity.Phone, entity.DocumentType, entity.DocumentNumber, entity.CreatedAt, entity.UpdatedAt);
            return Ok(dto);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            var entity = await _db.Clients.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            _db.Clients.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}

