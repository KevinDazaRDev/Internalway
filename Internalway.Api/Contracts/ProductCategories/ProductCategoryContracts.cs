using System;
using System.ComponentModel.DataAnnotations;

namespace Internalway.Api.Contracts.ProductCategories
{
    public record ProductCategoryDto(
        long ProductId,
        long CategoryId,
        DateTimeOffset CreatedAt
    );

    public record CreateProductCategoryRequest(
        [Range(1, long.MaxValue)] long ProductId,
        [Range(1, long.MaxValue)] long CategoryId
    );
}
