using System;

namespace Internalway.Api.Contracts.MovementLines
{
    public record MovementLineDto(
        long Id,
        long MovementId,
        long ProductId,
        decimal QuantityDelta,
        decimal? UnitPrice,
        DateTimeOffset CreatedAt
    );

    public record CreateMovementLineRequest(
        long MovementId,
        long ProductId,
        decimal QuantityDelta,
        decimal? UnitPrice
    );

    public record UpdateMovementLineRequest(
        decimal QuantityDelta,
        decimal? UnitPrice
    );
}
