using DotNetClub.Core.Model.Account;
using DotNetClub.Core.Resource;
using DotNetClub.Core.Service;
using DotNetClub.Web.ViewModels.Account;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DotNetClub.Web.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private AccountService AccountService { get; set; }

        public AccountController(AccountService accountService)
        {
            this.AccountService = accountService;
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
                return this.View(vm);
            }
            else
            {
                switch (result.ErrorCode)
                {
                    case LoginResult.LoginErrorCode.InvalidPassword:
                        vm.ErrorMessage = "";
                        break;
                    case LoginResult.LoginErrorCode.UserIsBlocked:
                        vm.ErrorMessage = "";
                        break;
                    case LoginResult.LoginErrorCode.UserNotActive:
                        vm.ErrorMessage = "";
                        break;
                    case LoginResult.LoginErrorCode.UserNotExist:
                        vm.ErrorMessage = "";
                        break;
                }

                return this.View(vm);
            }
        }
    }
}
