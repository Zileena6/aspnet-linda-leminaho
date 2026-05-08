using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreFitness.Web.TagHelpers;

[HtmlTargetElement("cf-navlink")]
public class NavLinkTagHelper(IUrlHelperFactory urlHelperFactory) : TagHelper
{

    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;
    public string? AspController { get; set; }
    public string? AspAction { get; set; }
    public string? Variant { get; set; }
    public string? CssClass { get; set; }
    
    [HtmlAttributeName(DictionaryAttributePrefix = "asp-route-")]
    public Dictionary<string, object>? RouteValues { get; set; } = [];

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();

        var urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

        var url = urlHelper.Action(AspAction, AspController, RouteValues) ?? "#";

        output.TagName = "a";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.SetAttribute("href", url);

        var baseClass = "transition-colors cursor-pointer";

        var variantClass = Variant switch
        {
            "primary" => "inline-flex justify-center items-center gap-2 bg-tertiary text-gray-900 font-semibold px-5 py-2 rounded-full hover:bg-lime-300 self-start",
            "nav" or null => "text-sm hover:text-lime-400",
            _ => ""
        };

        output.AddClasses(baseClass, variantClass);

        if(!string.IsNullOrWhiteSpace(CssClass))
            output.AddClass(CssClass, HtmlEncoder.Default);

        output.Content.SetHtmlContent(childContent);
    }
}
