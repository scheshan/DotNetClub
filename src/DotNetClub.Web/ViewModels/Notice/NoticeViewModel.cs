using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.Notice
{
    public class NoticeViewModel
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public MessageType Type { get; set; }

        public NoticeViewModel(string message, string title = null, MessageType type = MessageType.Danger)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            this.Message = message;
            this.Title = title ?? "提示";
            this.Type = type;
        }

        public enum MessageType
        {
            Success = 1,
            Info = 2,
            Warning = 3,
            Danger = 4
        }
    }
}
