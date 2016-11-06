using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Consts
{
    public sealed class RedisKeys
    {
        public const string TokenPrefix = "Token_";

        public const string User = "User";

        public const string TopicVisit = "TopicVisit";

        public static string GetUserMessageCacheKey(long userID)
        {
            return $"Message_User_{userID}";
        }
    }
}
