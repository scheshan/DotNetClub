using DotNetClub.Core.Model.Topic;
using DotNetClub.Core.Model.User;
using DotNetClub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.Message
{
    public class MessageModel
    {
        public long ID { get; set; }

        public MessageType Type { get; set; }

        public TopicBasicModel Topic { get; set; }

        public UserBasicModel FromUser { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsRead { get; set; }
    }
}
