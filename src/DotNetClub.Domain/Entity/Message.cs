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
        public int ID { get; set; }

        public MessageType Type { get; set; }

        public int? TopicID { get; set; }

        public int? CommentID { get; set; }

        public int? FromUserID { get; set; }

        public int ToUserID { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsRead { get; set; }
    }
}
