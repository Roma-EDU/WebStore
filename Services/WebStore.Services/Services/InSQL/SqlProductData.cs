using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.DTOs.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db) => _db = db;

        public IEnumerable<SectionDto> GetSections() => _db.Sections.Include(section => section.Products).ToDto();

        //public Section GetSectionById(int id) => _db.Sections.Find(id);
        public SectionDto GetSectionById(int id) => _db.Sections
           .Include(section => section.Products)
           .FirstOrDefault(s => s.Id == id)
           .ToDto();

        public IEnumerable<BrandDto> GetBrands() => _db.Brands.Include(brand => brand.Products).ToDto();

        public BrandDto GetBrandById(int id) => _db.Brands
           .Include(b => b.Products)
           .FirstOrDefault(b => b.Id == id)
           .ToDto();

        public IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (Filter?.Ids?.Length > 0)
                query = query.Where(product => Filter.Ids.Contains(product.Id));
            else
            {
                if (Filter?.BrandId != null)
                    query = query.Where(product => product.BrandId == Filter.BrandId);

                if (Filter?.SectionId != null)
                    query = query.Where(product => product.SectionId == Filter.SectionId);
            }

            return query.AsEnumerable().ToDto();
        }

        public ProductDto GetProductById(int id) => _db.Products
           .Include(p => p.Brand)
           .Include(p => p.Section)
           .FirstOrDefault(p => p.Id == id)
           .ToDto();
    }
}
