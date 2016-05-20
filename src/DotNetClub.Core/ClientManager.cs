using DotNetClub.Core.Data;
using DotNetClub.Core.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core
{
    public class ClientManager
    {
        private IConfiguration Configuration { get; set; }

        private ClubContext DbContext { get; set; }

        private HttpContext HttpContext { get; set; }

        private string CookieName
        {
            get
            {
                return this.Configuration["CookieName"];
            }
        }

        public User CurrentUser { get; private set; }

        public string Token { get; private set; }

        public bool IsLogin
        {
            get
            {
                return this.CurrentUser != null;
            }
        }

        public ClientManager(IConfiguration configuration, ClubContext dbContext)
        {
            this.Configuration = configuration;
            this.DbContext = dbContext;
        }

        public void Init(HttpContext context)
        {
            this.HttpContext = context;

            this.InitToken();
            this.InitUser();
        }

        private void InitToken()
        {
            string token = this.HttpContext.Request.Cookies[this.CookieName];
            if (string.IsNullOrWhiteSpace(token))
            {
                token = this.HttpContext.Request.Query["token"].FirstOrDefault();
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                token = this.HttpContext.Request.Headers["token"].FirstOrDefault();
            }

            this.Token = token;
        }

        private void InitUser()
        {
            if (string.IsNullOrWhiteSpace(this.Token))
            {
                return;
            }

            var user = this.DbContext.Users.SingleOrDefault(t => t.Token == this.Token && t.Active == true && t.IsBlock == false);
            this.CurrentUser = user;
        }
    }
}
