using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Model
{
    public class CommentVotes
    {
        public long CommentID { get; set; }

        public long Votes { get; set; }
    }
}
