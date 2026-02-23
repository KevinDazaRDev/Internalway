using System;
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
        long? ClientId,
        string? Notes,
        DateTimeOffset? OccurredAt
    );

    public record UpdateMovementRequest(
        MovementType Type,
        long? ClientId,
        string? Notes,
        DateTimeOffset? OccurredAt
    );
}
