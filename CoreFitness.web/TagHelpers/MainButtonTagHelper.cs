using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreFitness.Web.TagHelpers;

[HtmlTargetElement("cf-button")]
public class MainButtonTagHelper : TagHelper
{
    public string Text { get; set; } = string.Empty;
    public string Type { get; set; } = "button";
    public string? CssClass { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync(true);

        var content = !string.IsNullOrWhiteSpace(Text) ?
        HtmlEncoder.Default.Encode(Text) :
        childContent.GetContent();

        output.TagName = "button";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.SetAttribute("type", Type);

        output.Attributes.SetAttribute("class", "flex gap-2 items-center p-3 bg-tertiary w-fit rounded-full font-paragraph font-semibold text-black");

        output.Content.SetHtmlContent($"{content} <i class='fa-solid fa-chevron-right'></i>");
    }
}
