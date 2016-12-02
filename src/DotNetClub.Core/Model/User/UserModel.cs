using DotNetClub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.User
{
    public class UserModel : UserBasicModel
    {
        public string WebSite { get; set; }

        public string Location { get; set; }

        public DateTime CreateDate { get; set; }

        public UserStatus Status { get; set; }
    }
}
