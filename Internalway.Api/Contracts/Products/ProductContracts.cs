using System;

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
        long BrandId,
        string Name,
        string Slug,
        string? Sku,
        string? Description,
        decimal? ListPrice,
        string? Currency,
        bool IsActive
    );

    public record UpdateProductRequest(
        long BrandId,
        string Name,
        string Slug,
        string? Sku,
        string? Description,
        decimal? ListPrice,
        string? Currency,
        bool IsActive
    );
}
