using System;
using System.ComponentModel.DataAnnotations;
using Internalway.Domain.Enums;

namespace Internalway.Api.Contracts.Movements
{
    public record MovementDto(
        long Id,
        MovementType Type,
        long? ClientId,
        string? Notes,
        DateTimeOffset OccurredAt,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
    );

    public record CreateMovementRequest(
        MovementType Type,
        [Range(1, long.MaxValue)] long? ClientId,
        [MaxLength(2000)] string? Notes,
        DateTimeOffset? OccurredAt
    );

    public record UpdateMovementRequest(
        MovementType Type,
        [Range(1, long.MaxValue)] long? ClientId,
        [MaxLength(2000)] string? Notes,
        DateTimeOffset? OccurredAt
    );
}
