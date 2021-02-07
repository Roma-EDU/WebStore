using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTOs.Products;
using WebStore.Domain.Entities;

namespace WebStore.ViewModels
{
    public class BreadCrumbsViewModel
    {
        public SectionDto MasterSection { get; set; }

        public SectionDto Section { get; set; }

        public BrandDto Brand { get; set; }

        public string Product { get; set; }
    }
}
