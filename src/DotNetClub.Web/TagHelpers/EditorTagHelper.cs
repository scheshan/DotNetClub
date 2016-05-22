using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DotNetClub.Web.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("editor")]
    public class EditorTagHelper : TagHelper
    {
        public EditorTagHelper()
        {

        }

        public string Content { get; set; }

        public bool AppendScriptFile { get; set; }



        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

        }
    }
}
