using System;
using System.ComponentModel.DataAnnotations;

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
        [Range(1, long.MaxValue)] long? ParentId,
        [Required, MaxLength(200)] string Name,
        [Required, MaxLength(200)] string Slug,
        [MaxLength(1000)] string? Description
    );

    public record UpdateCategoryRequest(
        [Range(1, long.MaxValue)] long? ParentId,
        [Required, MaxLength(200)] string Name,
        [Required, MaxLength(200)] string Slug,
        [MaxLength(1000)] string? Description
    );
}
