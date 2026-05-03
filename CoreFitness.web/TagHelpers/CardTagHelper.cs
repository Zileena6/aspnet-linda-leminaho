using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreFitness.Web.TagHelpers;

[HtmlTargetElement("cf-card")]
public class CardTagHelper : TagHelper
{
    public string? CssClass { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync(true);

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.AddClasses("bg-white rounded-xl shadow-md p-6");

        if(!string.IsNullOrWhiteSpace(CssClass))
            output.AddClass(CssClass, HtmlEncoder.Default);

        output.Content.SetHtmlContent(childContent);
    }
}