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
    public class AccountController : ControllerBase
    {
        private AuthService AuthService { get; set; }

        public AccountController(AuthService authService)
        {
            this.AuthService = authService;
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

            var result = await this.AuthService.Register(model);

            if (result.Success)
            {
                Core.Security.SecurityManager.WriteToken(this.HttpContext, result.Data, false);

                return this.RedirectToAction("Index", "Home");
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

            var result = await this.AuthService.Login(model);

            if (result.Success)
            {
                Core.Security.SecurityManager.WriteToken(this.HttpContext, result.Data, model.RememberPassword);

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
            Core.Security.SecurityManager.ClearToken(this.HttpContext);
            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet("checkusername")]
        public async Task<IActionResult> CheckUserName(string userName)
        {
            bool result = await this.AuthService.IsUserNameRegistered(userName);

            return this.Content((!result).ToString().ToLower());
        }

        [HttpGet("checkemail")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            bool isEmailRegistered = await this.AuthService.IsEmailRegistered(email);

            return this.Content((!isEmailRegistered).ToString().ToLower());
        }
    }
}
