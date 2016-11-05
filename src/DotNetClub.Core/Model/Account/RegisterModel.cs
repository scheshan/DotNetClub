using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.Account
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9]+$")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [Compare("Password")]
        [MaxLength(20)]
        public string ConfirmPassword { get; set; }
    }
}
