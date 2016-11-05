using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Entity
{
    public class User
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Location { get; set; }

        public string Signature { get; set; }

        public bool Active { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsBlock { get; set; }
    }
}
