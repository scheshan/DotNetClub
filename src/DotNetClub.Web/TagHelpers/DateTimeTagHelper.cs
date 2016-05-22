using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DotNetClub.Web.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("datetime")]
    public class DateTimeTagHelper : TagHelper
    {
        [HtmlAttributeName("date")]
        public DateTime? Date { get; set; }

        [HtmlAttributeName("friendly")]
        public bool Friendly { get; set; }

        [HtmlAttributeName("format")]
        public string Format { get; set; }

        public DateTimeTagHelper()
        {
            this.Friendly = true;
            this.Format = "yyyy-MM-dd HH:mm:ss";
        }

        public override void Init(TagHelperContext context)
        {
            if(!this.Date.HasValue)
            {
                throw new ArgumentNullException("Date");
            }

            base.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";

            string content = null;

            if (!this.Friendly)
            {
                if (this.Format != null)
                {
                    content = this.Date.Value.ToString(this.Format);
                }
                else
                {
                    content = this.Date.Value.ToString();
                }
            }
            else
            {
                var ts = DateTime.Now - this.Date.Value;

                if (ts.TotalDays > 365)
                {
                    int years = Convert.ToInt32(ts.TotalDays / 365);
                    content = $"{years}年前";
                }
                else if (ts.TotalDays > 1)
                {
                    int days = Convert.ToInt32(ts.TotalDays);
                    content = $"{days}天前";
                }
                else if (ts.TotalHours > 1)
                {
                    int hours = Convert.ToInt32(ts.TotalHours);
                    content = $"{hours}小时前";
                }
                else if (ts.TotalMinutes > 1)
                {
                    int minutes = Convert.ToInt32(ts.TotalMinutes);
                    content = $"{minutes}分钟前";
                }
                else
                {
                    content = "刚刚";
                }
            }

            output.Content.SetContent(content);
        }
    }
}
