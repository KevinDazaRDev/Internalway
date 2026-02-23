using System;

namespace Internalway.Api.Contracts.ProductCategories
{
    public record ProductCategoryDto(
        long ProductId,
        long CategoryId,
        DateTimeOffset CreatedAt
    );

    public record CreateProductCategoryRequest(
        long ProductId,
        long CategoryId
    );
}
