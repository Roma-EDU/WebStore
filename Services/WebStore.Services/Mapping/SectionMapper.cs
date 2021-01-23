using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTOs.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class SectionMapper
    {
        public static SectionDto ToDto(this Section model) 
            => model is null ? null 
                             : new SectionDto(model.Id, model.Name, model.Order, model.ParentId, model.Products.Count);

        public static IEnumerable<SectionDto> ToDto(this IEnumerable<Section> models) => models?.Select(ToDto);
    }
}
