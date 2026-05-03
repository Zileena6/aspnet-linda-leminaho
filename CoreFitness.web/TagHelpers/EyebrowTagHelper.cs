using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreFitness.Web.TagHelpers;

[HtmlTargetElement("cf-eyebrow")]
public class EyebrowTagHelper : TagHelper
{
    public string? Text { get; set; }
    public string? CssClass { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync(true);

        var content = !string.IsNullOrWhiteSpace(Text) ?
        HtmlEncoder.Default.Encode(Text) :
        childContent.GetContent();

        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.AddClasses("inline-flex items-center justify-center gap-2 text-center bg-black text-white px-3 py-1 rounded-2xl text-sm font-semibold font-paragraph h-fit w-fit");

        if(!string.IsNullOrWhiteSpace(CssClass))
            output.AddClass(CssClass, HtmlEncoder.Default);

        output.Content.SetHtmlContent($"<span class='w-1.5 h-1.5 bg-tertiary rounded-full inline-block'></span>{content}");
    }
}
