using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.Auth
{
    public class RegisterModel
    {
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-Za-z0-9\u4e00-\u9fa5]+$")]
        public string UserName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
