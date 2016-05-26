using DotNetClub.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.My
{
    public class MessagesViewModel
    {
        public List<Message> UnreadMessageList { get; set; }

        public List<Message> HistoryMessageList { get; set; }
    }
}
