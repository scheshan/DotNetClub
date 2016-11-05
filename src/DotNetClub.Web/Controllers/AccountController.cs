using DotNetClub.Core;
using DotNetClub.Core.Model.Auth;
using DotNetClub.Core.Resource;
using DotNetClub.Core.Service;
using DotNetClub.Web.ViewModels.Account;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DotNetClub.Web.Controllers
{
    [Route("account")]
    public class AccountController : Base.ControllerBase
    {
        private AuthService AccountService { get; set; }

        private ClientManager ClientManager { get; set; }

        public AccountController(AuthService accountService, ClientManager clientManager)
        {
            this.AccountService = accountService;
            this.ClientManager = clientManager;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            var vm = new RegisterViewModel();
            vm.Model = new RegisterModel();

            return this.View(vm);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var vm = new RegisterViewModel
            {
                Model = model
            };

            if (!ModelState.IsValid)
            {
                vm.ErrorMessage = Messages.ModelStateNotValid;
                return this.View(vm);
            }

            var result = await this.AccountService.Register(model);

            if (result.Success)
            {
                return this.RedirectToAction("RegisterSuccess", "Account");
            }
            else
            {
                vm.ErrorMessage = result.ErrorMessage;
                return this.View(vm);
            }
        }

        [HttpGet("regsuccess")]
        public IActionResult RegisterSuccess()
        {
            return this.View();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var vm = new LoginViewModel();
            vm.Model = new LoginModel();

            return this.View(vm);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var vm = new LoginViewModel();
            vm.Model = model;

            if (!ModelState.IsValid)
            {
                vm.ErrorMessage = Messages.ModelStateNotValid;
                return this.View(vm);
            }

            var result = await this.AccountService.Login(model);

            if (result.Success)
            {
                //this.Response.Cookies.Append(this.ClientManager.CookieName, result.Token, new CookieOptions { Expires = DateTime.Now.AddDays(30) });

                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                vm.ErrorMessage = result.ErrorMessage;

                return this.View(vm);
            }
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            this.Response.Cookies.Delete(this.ClientManager.CookieName);
            return this.RedirectToAction("Index", "Home");
        }
    }
}
