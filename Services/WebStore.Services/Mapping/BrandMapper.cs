using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTOs.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class BrandMapper
    {
        public static BrandDto ToDto(this Brand model)
            => model is null ? null
                             : new BrandDto(model.Id, model.Name, model.Order, model.Products.Count);

        public static IEnumerable<BrandDto> ToDto(this IEnumerable<Brand> models) => models?.Select(ToDto);
    }
}
