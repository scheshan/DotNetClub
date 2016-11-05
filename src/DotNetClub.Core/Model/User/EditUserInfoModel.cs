using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.User
{
    public class EditUserInfoModel
    {
        [StringLength(100)]
        public string WebSite { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        [StringLength(200)]
        public string Signature { get; set; }
    }
}
