using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.User
{
    public class EditPasswordModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
}
