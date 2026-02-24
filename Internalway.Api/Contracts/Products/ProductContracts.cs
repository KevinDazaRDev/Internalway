using System;
using System.ComponentModel.DataAnnotations;

namespace Internalway.Api.Contracts.Products
{
    public record ProductDto(
        long Id,
        long BrandId,
        string Name,
        string Slug,
        string? Sku,
        string? Description,
        decimal? ListPrice,
        string? Currency,
        bool IsActive,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
    );

    public record CreateProductRequest(
        [Range(1, long.MaxValue)] long BrandId,
        [Required, MaxLength(200)] string Name,
        [Required, MaxLength(200)] string Slug,
        [MaxLength(64)] string? Sku,
        [MaxLength(2000)] string? Description,
        [Range(0, 9999999999.99)] decimal? ListPrice,
        [MaxLength(3)] string? Currency,
        bool IsActive
    );

    public record UpdateProductRequest(
        [Range(1, long.MaxValue)] long BrandId,
        [Required, MaxLength(200)] string Name,
        [Required, MaxLength(200)] string Slug,
        [MaxLength(64)] string? Sku,
        [MaxLength(2000)] string? Description,
        [Range(0, 9999999999.99)] decimal? ListPrice,
        [MaxLength(3)] string? Currency,
        bool IsActive
    );
}
