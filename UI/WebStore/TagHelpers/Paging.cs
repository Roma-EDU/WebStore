using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebStore.Domain.ViewModels;

namespace WebStore.TagHelpers
{
    public class Paging : TagHelper
    {
        private readonly IUrlHelperFactory _factory;

        public Paging(IUrlHelperFactory factory)
        {
            _factory = factory;
        }


        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PageViewModel PageModel { get; set; }

        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            if (PageModel.TotalPages < 2)
                return;

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            var helper = _factory.GetUrlHelper(ViewContext);
            for (int i = 1; i < PageModel.TotalPages; i++)
                ul.InnerHtml.AppendHtml(CreateElement(i, helper));

            output.Content.AppendHtml(ul);
        }

        private TagBuilder CreateElement(int pageNumber, IUrlHelper urlHelper)
        {
            var li = new TagBuilder("li");
            var a = new TagBuilder("a");

            if (pageNumber == PageModel.PageNumber)
                li.AddCssClass("active");
            else
            {
                PageUrlValues["page"] = pageNumber;
                a.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            }

            a.InnerHtml.AppendHtml(pageNumber.ToString());
            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}
