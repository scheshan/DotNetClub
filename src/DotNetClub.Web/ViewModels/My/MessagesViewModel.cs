using DotNetClub.Core.Model.Message;
using System.Collections.Generic;

namespace DotNetClub.Web.ViewModels.My
{
    public class MessagesViewModel
    {
        public List<MessageModel> UnreadMessageList { get; set; }

        public List<MessageModel> HistoryMessageList { get; set; }
    }
}
