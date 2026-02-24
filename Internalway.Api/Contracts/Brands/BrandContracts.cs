using System;
using System.ComponentModel.DataAnnotations;

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
        [Required, MaxLength(200)] string Name,
        [Required, MaxLength(200)] string Slug,
        [MaxLength(1000)] string? Description
    );

    public record UpdateBrandRequest(
        [Required, MaxLength(200)] string Name,
        [Required, MaxLength(200)] string Slug,
        [MaxLength(1000)] string? Description
    );
}
