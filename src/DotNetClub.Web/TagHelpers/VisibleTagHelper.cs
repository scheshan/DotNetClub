using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DotNetClub.Web.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement(Attributes = "condition")]
    [HtmlTargetElement("visible")]
    public class VisibleTagHelper : TagHelper
    {
        [HtmlAttributeName("condition")]
        public bool Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (output.TagName == "visible")
            {
                output.TagName = "";
            }

            if (!this.Condition)
            {
                output.TagName = "";
                output.Content.SetHtmlContent("");
            }
            else
            {
                base.Process(context, output);
            }
        }
    }
}
