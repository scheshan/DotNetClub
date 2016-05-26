using DotNetClub.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Entity
{
    public class Message
    {
        public int ID { get; set; }

        public MessageType Type { get; set; }

        public int? TopicID { get; set; }

        public int? CommentID { get; set; }

        public int? FromUserID { get; set; }

        public int ToUserID { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsRead { get; set; }

        public virtual Topic Topic { get; set; }

        public virtual Comment Comment { get; set; }

        public virtual User FromUser { get; set; }

        public virtual User ToUser { get; set; }
    }
}
