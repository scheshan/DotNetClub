using DotNetClub.Domain.Enums;
using Share.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Entity
{
    public class Message : IEntity
    {
        public long ID { get; set; }

        public MessageType Type { get; set; }

        public long? TopicID { get; set; }

        public long? CommentID { get; set; }

        public long? FromUserID { get; set; }

        public long ToUserID { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsRead { get; set; }
    }
}
