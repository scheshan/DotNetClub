using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Enums
{
    public enum UserStatus : byte
    {
        Verifying = 0,

        Active = 1,

        Deny = 2
    }
}
