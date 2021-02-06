using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;
using WebStore.ViewModels;

namespace WebStore.Components
{
    //[ViewComponent(Name = "Sections")]
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        //После компиляции входной параметр 'sectionId' появляется в представлении как 'section-id' (см. _LeftSideBar.cshtml)
        public IViewComponentResult Invoke(string sectionId)
        {
            var id = int.TryParse(sectionId, out var sectId) ? sectId : (int?)null;
            var sectionVMs = GetSections(id, out var parentId);

            //Возвращаем на представление найденные значения
            /*
            ViewBag.SectionId = id;
            ViewData["ParentSectionId"] = parentId;
            return View(sectionVMs);
            */
            var viewModel = new SelectableSectionsViewModel()
            {
                Sections = sectionVMs,
                SectionId = id,
                ParentSectionId = parentId,
            };
            return View(viewModel);
        }

        private IEnumerable<SectionViewModel> GetSections(int? sectionId, out int? parentSectionId)
        {
            parentSectionId = null;

            var sections = _ProductData.GetSections().ToArray();

            var parent_sections = sections.Where(s => s.ParentId is null);

            var parent_section_views = parent_sections
               .Select(s => new SectionViewModel
               {
                   Id = s.Id,
                   Name = s.Name,
                   Order = s.Order,
                   ProductsCount = s.ProductsCount,
               })
               .ToList();

            foreach (var parent_section in parent_section_views)
            {
                var childs = sections.Where(s => s.ParentId == parent_section.Id);

                foreach (var child_section in childs)
                {
                    if (child_section.Id == sectionId)
                        parentSectionId = child_section.ParentId;

                    parent_section.ChildSections.Add(new SectionViewModel
                    {
                        Id = child_section.Id,
                        Name = child_section.Name,
                        Order = child_section.Order,
                        ParentSection = parent_section,
                        ProductsCount = child_section.ProductsCount,
                    });
                }

                parent_section.ChildSections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }

            parent_section_views.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

            return parent_section_views;
        }


        //public async Task<IViewComponentResult> InvokeAsync() => View();
    }
}
