using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.Comment
{
    public class AddCommentModel
    {
        public int TopicID { get; set; }

        [Required(ErrorMessage = "内容不能为空")]
        public string Content { get; set; }

        public int? ReplyTo { get; set; }
    }
}
