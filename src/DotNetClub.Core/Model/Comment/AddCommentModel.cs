using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.Comment
{
    public class AddCommentModel
    {
        public long TopicID { get; set; }

        [Required]
        public string Content { get; set; }

        public long? ReplyTo { get; set; }
    }
}
