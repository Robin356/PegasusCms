using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PegasusCms.Models.Page;

namespace PegasusCms.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent MenuItem(this IHtmlHelper html, MenuItem menuItem, string cssClass = null)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("nav-item");
            var link = new TagBuilder("a");
            link.AddCssClass("nav-link");
            if (menuItem.Active)
            {
                link.AddCssClass("active");
            }
            if (cssClass != null)
            {
                link.AddCssClass(cssClass);
            }
            link.MergeAttribute("href", menuItem.Url);
            link.InnerHtml.SetContent(menuItem.Title);
            li.InnerHtml.AppendHtml(link);

            if (menuItem.SubMenuItems != null)
            {
                var ul = new TagBuilder("ul");
                ul.AddCssClass("list-unstyled");
                foreach (var subMenuItem in menuItem.SubMenuItems)
                {
                    var subMenu = MenuItem(html, subMenuItem, cssClass);
                    ul.InnerHtml.AppendHtml(subMenu);
                }
                li.InnerHtml.AppendHtml(ul);
            }
            return li;
        }
    }
}
