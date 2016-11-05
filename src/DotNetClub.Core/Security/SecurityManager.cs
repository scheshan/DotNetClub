using DotNetClub.Core.Service;
using DotNetClub.Domain.Consts;
using DotNetClub.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Share.Infrastructure.Redis;
using System;
using System.Linq;

namespace DotNetClub.Core.Security
{
    public sealed class SecurityManager
    {
        private const string TOKEN_KEY = "token";

        private object _sync = new object();

        private bool _loaded = false;

        private IHttpContextAccessor HttpContextAccessor { get; set; }

        private IRedisProvider RedisProvider { get; set; }

        private IServiceProvider ServiceProvider { get; set; }

        private User _user;

        public User CurrentUser
        {
            get
            {
                if (!_loaded)
                {
                    lock (_sync)
                    {
                        if (!_loaded)
                        {
                            this.LoadUser();
                            _loaded = true;
                        }
                    }
                }

                return _user;
            }
        }

        public string Token { get; private set; }

        public bool IsLogin
        {
            get
            {
                return this.CurrentUser != null;
            }
        }

        public SecurityManager(IHttpContextAccessor httpContextAccessor, IRedisProvider redisProvider, IServiceProvider serviceProvider)
        {
            this.HttpContextAccessor = httpContextAccessor;
            this.RedisProvider = redisProvider;
            this.ServiceProvider = serviceProvider;
        }

        private void LoadUser()
        {
            this.InitToken();
            if (string.IsNullOrWhiteSpace(this.Token))
            {
                return;
            }

            var redis = this.RedisProvider.GetDatabase();
            string tokenKey = $"{RedisKeys.TokenPrefix}{this.Token}";
            var id = redis.StringGet(tokenKey);

            if (id.HasValue)
            {
                _user = redis.JsonHashGet<User>(RedisKeys.User, id);
            }
        }

        private void InitToken()
        {
            if (this.HttpContextAccessor.HttpContext == null)
            {
                return;
            }            

            string token = this.HttpContextAccessor.HttpContext.Request.Query[TOKEN_KEY];
            if (string.IsNullOrWhiteSpace(token))
            {
                if (this.HttpContextAccessor.HttpContext.Request.Headers.ContainsKey(TOKEN_KEY))
                {
                    token = this.HttpContextAccessor.HttpContext.Request.Headers[TOKEN_KEY].FirstOrDefault();
                }
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                token = this.HttpContextAccessor.HttpContext.Request.Cookies[TOKEN_KEY];
            }

            this.Token = token;
        }

        public static void WriteToken(HttpContext context, string token, bool rememberPassword)
        {
            var cookieOptions = new CookieOptions();
            if (rememberPassword)
            {
                cookieOptions.Expires = DateTime.Now.AddDays(30);
            }

            context.Response.Cookies.Append(TOKEN_KEY, token, cookieOptions);
        }
    }
}
