using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.DTOs.Products
{
    public record ProductDto(
        int Id, 
        string Name, 
        int Order,
        decimal Price,
        string ImageUrl,
        BrandDto Brand,
        SectionDto Section);
}
