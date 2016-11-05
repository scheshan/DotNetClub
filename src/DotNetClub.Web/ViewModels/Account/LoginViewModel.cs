using DotNetClub.Core.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.Account
{
    public class LoginViewModel
    {
        public LoginModel Model { get; set; }

        public string ErrorMessage { get; set; }
    }
}
