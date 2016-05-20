using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.Account
{
    public class LoginResult
    {
        public bool Success { get; set; }

        public string Token { get; set; }

        public LoginErrorCode? ErrorCode { get; set; }

        public enum LoginErrorCode
        {
            UserNotExist = 1,

            InvalidPassword = 2,

            UserNotActive = 3,

            UserIsBlocked = 4
        }
    }
}
