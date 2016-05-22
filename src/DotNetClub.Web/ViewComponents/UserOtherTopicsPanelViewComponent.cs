using DotNetClub.Core.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewComponents
{
    public class UserOtherTopicsPanelViewComponent : ViewComponent
    {
        private TopicService TopicService { get; set; }

        public UserOtherTopicsPanelViewComponent(TopicService topicService)
        {
            this.TopicService = topicService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userID, int count, params int[] exclude)
        {
            var topicList = await this.TopicService.QueryByUser(count, userID, exclude);

            return this.View(topicList);
        }
    }
}
