using System;
using System.ComponentModel.DataAnnotations;

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
        [Range(1, long.MaxValue)] long MovementId,
        [Range(1, long.MaxValue)] long ProductId,
        [Range(-9999999999.9999, 9999999999.9999)] decimal QuantityDelta,
        [Range(0, 9999999999.99)] decimal? UnitPrice
    );

    public record UpdateMovementLineRequest(
        [Range(-9999999999.9999, 9999999999.9999)] decimal QuantityDelta,
        [Range(0, 9999999999.99)] decimal? UnitPrice
    );
}
