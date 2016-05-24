using DotNetClub.Core;
using DotNetClub.Core.Model.Account;
using DotNetClub.Core.Resource;
using DotNetClub.Core.Service;
using DotNetClub.Web.ViewModels.Account;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
        private AccountService AccountService { get; set; }

        private ClientManager ClientManager { get; set; }

        public AccountController(AccountService accountService, ClientManager clientManager)
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

            var result = await this.AccountService.Register(model.UserName, model.Password, model.Email);

            if (result.Success)
            {
                return this.RedirectToAction("RegisterSuccess", "Account");
            }
            else
            {
                switch(result.ErrorCode)
                {
                    case RegisterResult.RegisterErrorCode.EmailExist:
                        vm.ErrorMessage = "该Email已被注册";
                        break;
                    case RegisterResult.RegisterErrorCode.UserNameExist:
                        vm.ErrorMessage = "该用户名已被注册";
                        break;
                }
                
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

            var result = await this.AccountService.Login(model.UserName, model.Password);

            if (result.Success)
            {
                this.Response.Cookies.Append(this.ClientManager.CookieName, result.Token, new CookieOptions { Expires = DateTime.Now.AddDays(30) });

                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                switch (result.ErrorCode)
                {
                    case LoginResult.LoginErrorCode.InvalidPassword:
                        vm.ErrorMessage = "密码错误";
                        break;
                    case LoginResult.LoginErrorCode.UserIsBlocked:
                        vm.ErrorMessage = "用户已被禁用";
                        break;
                    case LoginResult.LoginErrorCode.UserNotActive:
                        vm.ErrorMessage = "用户未激活";
                        break;
                    case LoginResult.LoginErrorCode.UserNotExist:
                        vm.ErrorMessage = "用户不存在";
                        break;
                }

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
