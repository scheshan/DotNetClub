using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.TagHelpers
{
    [HtmlTargetElement(Attributes = CONDITION_ATTRIBUTE_NAME)]
    public class ConditionalClassTagHelper : TagHelper
    {
        private const string CONDITION_ATTRIBUTE_NAME = "class-condition";

        [HtmlAttributeName(CONDITION_ATTRIBUTE_NAME)]
        public bool Condition { get; set; }

        [HtmlAttributeName("class-true")]
        public string TrueClass { get; set; }

        [HtmlAttributeName("class-false")]
        public string FalseClass { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            var existAttribute = output.Attributes["class"];
            if (existAttribute != null)
            {
                sb.Append(existAttribute.Value.ToString());
                sb.Append(" ");
            }

            if (this.Condition)
            {
                if (!string.IsNullOrWhiteSpace(this.TrueClass))
                {
                    sb.Append(this.TrueClass);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(this.FalseClass))
                {
                    sb.Append(this.FalseClass);
                }
            }

            if (sb.Length > 0)
            {
                output.Attributes.SetAttribute("class", sb.ToString());
            }
        }
    }
}
