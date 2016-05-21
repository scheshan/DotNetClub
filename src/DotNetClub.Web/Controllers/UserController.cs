using DotNetClub.Core.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private UserService UserService { get; set; }

        public UserController(UserService userService)
        {
            this.UserService = userService;
        }

        [HttpGet("{userName}")]
        public IActionResult Index(string userName)
        {
            var user = this.UserService.Get(userName);

            ViewBag.User = user;

            return this.View();
        }
    }
}
