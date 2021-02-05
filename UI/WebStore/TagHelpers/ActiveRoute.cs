using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebStore.TagHelpers
{
    //Все TagHelper импортируются в _ViewImports.cshtml: @addTagHelper *, WebStore

    [HtmlTargetElement(Attributes = TagAttributes)] //TagHelper будет реагировать на атрибуты с таким именем (можно несколько через запятую "is-active-route,some-other-name")
    public class ActiveRoute : TagHelper
    {
        //"ws-" наш собственный префикс, чтобы не путаться с существующими атрибутами
        private const string TagAttributes = "ws-is-active-route";

        //Не проверяем действие, только контроллер
        private const string IgnoreActionAttribute = "ws-ignore-action";

        //Имя класса, который в css стилях описывает активный (подсвечиваемый) элемент
        private const string ActiveClass = "active";

        //Возьмём из разметки вспомогательный параметр - название действия
        [HtmlAttributeName("asp-action")]
        public string ControllerAction { get; set; }

        //Возьмём из разметки вспомогательный параметр - название контроллера
        [HtmlAttributeName("asp-controller")]
        public string ControllerName { get; set; }

        //Дополнительные параметры из разметки: 
        // - либо значение атрибута "asp-all-route-data"
        // - либо взять из атрибута с префиксом "asp-route-", например asp-route-CategoryId 
        // (CategoryId будет ключом словаря, а значение атрибута - значением в словаре)
        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public Dictionary<string, string> RouteValues { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        //Сюда будет записан текущий контекст, свойство не привязано к html-разметке
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var ignoreAction = output.Attributes.ContainsName(IgnoreActionAttribute);
            if (IsActiveRoute(ignoreAction))
                MakeActive(output);

            //Удаляем из разметки обработанный тэг
            output.Attributes.RemoveAll(TagAttributes);
            output.Attributes.RemoveAll(IgnoreActionAttribute);
        }

        private bool IsActiveRoute(bool ignoreAction)
        {
            var actualRouteValues = ViewContext.RouteData.Values;
            var actualController = actualRouteValues["Controller"]?.ToString();
            var actualAction = actualRouteValues["Action"]?.ToString();

            if (!string.IsNullOrEmpty(ControllerName))
            {
                if (!IsEqualsIgnoreCase(ControllerName, actualController))
                    return false;
            }

            if (!ignoreAction && !string.IsNullOrEmpty(ControllerAction))
            {
                if (!IsEqualsIgnoreCase(ControllerAction, actualAction))
                    return false;
            }

            foreach (var (key, value) in RouteValues)
            {
                //Должны быть указаны все необходимые параметры маршрута и их значения совпадать с указанными в RouteValues
                if (!actualRouteValues.TryGetValue(key, out var actualValue) || !IsEqualsIgnoreCase(value, actualValue?.ToString()))
                    return false;
            }


            return true;
        }

        private bool IsEqualsIgnoreCase(string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
        }

        private void MakeActive(TagHelperOutput output)
        {
            var classAttribute = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (classAttribute is null)
                output.Attributes.Add("class", ActiveClass);
            else
            {
                var classValue = classAttribute.Value.ToString();
                if (classValue.Contains(ActiveClass))
                    return; //И так уже активен

                output.Attributes.SetAttribute("class", $"{classValue} {ActiveClass}");
            }
        }

       
    }
}
