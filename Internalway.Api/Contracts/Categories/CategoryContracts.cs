using System;

namespace Internalway.Api.Contracts.Categories
{
    public record CategoryDto(
        long Id,
        long? ParentId,
        string Name,
        string Slug,
        string? Description,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
    );

    public record CreateCategoryRequest(
        long? ParentId,
        string Name,
        string Slug,
        string? Description
    );

    public record UpdateCategoryRequest(
        long? ParentId,
        string Name,
        string Slug,
        string? Description
    );
}
