using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreFitness.Web.TagHelpers;

[HtmlTargetElement("cf-section")]
public class SectionTagHelper : TagHelper
{
    public string? Variant { get; set; }
    public string? CssClass { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync(true);

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var baseClass = "max-w-7xl flex flex-col h-fit rounded-2xl";

        var variantClass = Variant switch
        {
            "dark" => "bg-gray-900",
            "light" => "bg-white",
            _ => ""
        };

        output.AddClasses(baseClass, variantClass);

        if(!string.IsNullOrWhiteSpace(CssClass))
            output.AddClass(CssClass, HtmlEncoder.Default);

        output.Content.SetHtmlContent(childContent);
    }
}
