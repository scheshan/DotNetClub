using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using CommonMark;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetClub.Web.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("markdown")]
    public class MarkdownTagHelper : TagHelper
    {
        [HtmlAttributeName("content")]
        public ModelExpression Content { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            string content = null;

            if (this.Content != null)
            {
                content = this.Content.Model.ToString();
            }
            else
            {
                content = (await output.GetChildContentAsync()).GetContent();
            }

            output.TagName = "";

            string html = CommonMarkConverter.Convert(content);

            output.Content.SetHtmlContent(html);
        }
    }
}
