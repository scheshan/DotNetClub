using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.Topic
{
    public class CommentItemModel
    {
        public Core.Entity.Comment Entity { get; set; }

        public bool CanOperate { get; set; }

        public bool Voted { get; set; }

        public CommentItemModel(Core.Entity.Comment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            this.Entity = entity;
        }
    }
}
