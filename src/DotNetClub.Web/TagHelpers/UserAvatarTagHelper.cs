using Microsoft.AspNetCore.Razor.TagHelpers;
using Shared.Infrastructure.Utilities;
using System;

namespace DotNetClub.Web.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("useravatar")]
    public class UserAvatarTagHelper : TagHelper
    {
        [HtmlAttributeName("email")]
        public string Email { get; set; }

        [HtmlAttributeName("size")]
        public int Size { get; set; }

        [HtmlAttributeName("alt")]
        public string Alt { get; set; }

        [HtmlAttributeName("title")]
        public string Title { get; set; }

        public UserAvatarTagHelper()
        {
            this.Size = 48;
        }

        public override void Init(TagHelperContext context)
        {
            if (string.IsNullOrWhiteSpace(this.Email))
            {
                throw new ArgumentNullException("Email");
            }

            base.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            output.TagMode = TagMode.SelfClosing;

            string src = $"http://gravatar.com/avatar/{EncryptHelper.EncryptMD5(this.Email)}?size={this.Size}";

            output.Attributes.Add("src", src);
            output.Attributes.Add("title", this.Title);
            output.Attributes.Add("alt", this.Alt);
        }
    }
}
