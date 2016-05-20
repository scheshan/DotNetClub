using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.Account
{
    public class RegisterResult
    {
        public bool Success { get; set; }

        public RegisterErrorCode? ErrorCode { get; set; }

        public enum RegisterErrorCode
        {
            UserNameExist = 1,
            EmailExist = 2
        }
    }
}
