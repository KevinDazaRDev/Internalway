using System;

namespace Internalway.Api.Contracts.Brands
{
    public record BrandDto(
        long Id,
        string Name,
        string Slug,
        string? Description,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
    );

    public record CreateBrandRequest(
        string Name,
        string Slug,
        string? Description
    );

    public record UpdateBrandRequest(
        string Name,
        string Slug,
        string? Description
    );
}
