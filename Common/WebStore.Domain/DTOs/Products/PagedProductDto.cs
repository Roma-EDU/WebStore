using System.Collections.Generic;

namespace WebStore.Domain.DTOs.Products
{
    public record PagedProductDto(
        IEnumerable<ProductDto> Products, 
        int TotalCount);
}
