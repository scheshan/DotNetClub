using DotNetClub.Core.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewComponents
{
    public class NoCommentsTopicViewComponent : ViewComponent
    {
        private TopicService TopicService { get; set; }

        public NoCommentsTopicViewComponent(TopicService topicService)
        {
            this.TopicService = topicService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            if(count < 1)
            {
                count = 10;
            }

            var topicList = await this.TopicService.QueryNoComment(count);

            return this.View(topicList);
        }
    }
}
