using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Text.Encodings.Web;

namespace DotNetClub.Web.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        [HtmlAttributeName("url")]
        public IUrlHelper Url { get; set; }

        private IHtmlGenerator HtmlGenerator { get; set; }

        [HtmlAttributeName("page-index")]
        public int PageIndex { get; set; }

        [HtmlAttributeName("page-size")]
        public int PageSize { get; set; }

        [HtmlAttributeName("total")]
        public int Total { get; set; }

        private IDictionary<string, string> _routeValues;

        [HtmlAttributeName("all-route-data", DictionaryAttributePrefix = "route-")]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (_routeValues == null)
                {
                    _routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }
                return _routeValues;
            }
            set
            {
                _routeValues = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Rendering.ViewContext"/> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public PagerTagHelper(IHtmlGenerator htmlGenerator)
        {
            this.HtmlGenerator = htmlGenerator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (this.Total < 0)
            {
                return;
            }

            if (PageIndex <= 0 || PageSize <= 0)
            {
                this.SetErrorMessage(output, "错误的分页参数");
                return;
            }

            var totalPage = this.Total / this.PageSize;
            if (this.Total % this.PageSize > 0)
            {
                totalPage++;
            }

            if (this.PageIndex > totalPage)
            {
                return;
            }

            var start = this.PageIndex - 4;
            if (start <= 0)
            {
                start = 1;
            }

            var end = start + 7;
            if (end > totalPage)
            {
                end = totalPage;
            }

            output.TagName = "ul";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "pagination pagination-sm");

            StringBuilder sb = new StringBuilder();

            if (this.PageIndex > 1)
            {
                sb.Append($"<li>{this.GetPageLink(1, "首页")}</li>");
                sb.Append($"<li>{this.GetPageLink(this.PageIndex - 1, "上一页")}</li>");
            }
            for (var i = start; i <= end; i++)
            {
                if (this.PageIndex == i)
                {
                    sb.Append($"<li class='active'><a href='javascript:void(0)'>{i}</a></li>");
                }
                else
                {
                    sb.Append($"<li>{this.GetPageLink(i, i.ToString())}</li>");
                }
            }
            if (this.PageIndex < totalPage)
            {
                sb.Append($"<li>{this.GetPageLink(this.PageIndex + 1, "下一页")}</li>");
                sb.Append($"<li>{this.GetPageLink(totalPage, "末页")}</li>");
            }

            output.Content.SetHtmlContent(sb.ToString());
        }

        private void SetErrorMessage(TagHelperOutput output, string message)
        {
            output.TagName = "div";
            output.Content.SetContent(message);
        }

        private string GetPageLink(int page, string text)
        {
            var routes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (this.ViewContext.RouteData.Values.Count > 0)
            {
                foreach (var value in this.ViewContext.RouteData.Values)
                {
                    routes[value.Key] = value.Value;
                }
            }
            if (this.RouteValues.Count > 0)
            {
                foreach (var value in this.RouteValues)
                {
                    routes[value.Key] = value.Value;
                }
            }

            routes["Page"] = page;

            string action = ViewContext.ActionDescriptor.AttributeRouteInfo.Name;
            if (routes["Action"] != null)
            {
                action = routes["Action"].ToString();
            }

            string controller = null;
            if (routes["Controller"] != null)
            {
                controller = routes["Controller"].ToString();
            }

            StringBuilder sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
            {
                this.HtmlGenerator.GenerateActionLink(this.ViewContext, text, action, controller, null, null, null, routes, null).WriteTo(writer, HtmlEncoder.Default);
                return sb.ToString();
            }
        }
    }
}
