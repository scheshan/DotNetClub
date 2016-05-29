using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.TagHelpers
{
    public class TitleTagHelper : TagHelper
    {
        private IConfiguration Configuration { get; set; }

        private string SiteTitle { get; set; }

        public TitleTagHelper(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public override void Init(TagHelperContext context)
        {
            this.SiteTitle = this.Configuration["SiteTitle"];

            base.Init(context);
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            string title = this.SiteTitle;

            var content = (await output.GetChildContentAsync()).GetContent();

            if (!string.IsNullOrWhiteSpace(content))
            {
                title = $"{content} - {title}";
            }

            output.Content.SetHtmlContent(title);
        }
    }
}
