﻿using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTOs.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Mapping
{
    public static class ProductMapper
    {
        public static ProductViewModel ToView(this ProductDto p) => new ProductViewModel()
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            Brand = p.Brand?.Name
        };

        public static IEnumerable<ProductViewModel> ToView(this IEnumerable<ProductDto> p) => p.Select(ToView);

        public static ProductDto ToDto(this Product model)
            => model is null ? null
                             : new ProductDto(model.Id, model.Name, model.Order, model.Price, model.ImageUrl, model.Brand.ToDto(), model.Section.ToDto());

        public static IEnumerable<ProductDto> ToDto(this IEnumerable<Product> models) => models?.Select(ToDto);
    }
}
