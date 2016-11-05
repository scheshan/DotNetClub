using DotNetClub.Core.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.Comment
{
    public class CommentModel
    {
        public long ID { get; set; }

        public string Content { get; set; }

        public UserBasicModel CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public CommentModel ReplyTo { get; set; }

        public bool ReplyToIsDelete { get; set; }

        public long Votes { get; set; }

        public bool Voted { get; set; }
    }
}
