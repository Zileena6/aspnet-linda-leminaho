using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreFitness.Web.TagHelpers;

public static class TagHelperExtensions
{
    public static void AddClasses(this TagHelperOutput output, params string[] classes)
    {
        // var final = string.Join(" ", classes.Where(c => !string.IsNullOrWhiteSpace(c)).SelectMany(c => c.Split(' ', StringSplitOptions.RemoveEmptyEntries)));

        var final = classes
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .SelectMany(c => c.Split(' ', StringSplitOptions.RemoveEmptyEntries));

        // if(string.IsNullOrWhiteSpace(final))
        //     return;

        foreach (var c in final)
        {
            output.AddClass(c, HtmlEncoder.Default);            
        }

    }
}
