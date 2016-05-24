using DotNetClub.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core
{
    public static class ClientManagerExtensions
    {
        public static bool CanOperateTopic(this ClientManager clientManager, Topic topic)
        {
            bool canOperate = clientManager.IsAdmin || (clientManager.IsLogin && clientManager.CurrentUser.ID == topic.CreateUserID);
            return canOperate;
        }

        public static bool CanOperateComment(this ClientManager clientManager, Comment comment)
        {
            bool canOperate = clientManager.IsAdmin || (clientManager.IsLogin && clientManager.CurrentUser.ID == comment.CreateUserID);
            return canOperate;
        }
    }
}
