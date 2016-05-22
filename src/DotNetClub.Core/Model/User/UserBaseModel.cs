using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.User
{
    public class UserBaseModel
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string Avatar { get; set; }

        public string Signature { get; set; }
    }
}
