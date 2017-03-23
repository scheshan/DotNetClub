using DotNetClub.Core.Model.Comment;
using DotNetClub.Core.Model.Configuration;
using DotNetClub.Core.Model.Topic;
using DotNetClub.Core.Model.User;
using DotNetClub.Core.Service;
using DotNetClub.Domain.Consts;
using DotNetClub.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using DotNetClub.Core.Redis;
using System;
using System.Linq;

namespace DotNetClub.Core.Security
{
    public sealed class SecurityManager
    {
        private const string TOKEN_KEY = "token";

        private object _sync = new object();

        private bool _loaded = false;

        private IServiceProvider ServiceProvider { get; set; }

        private IHttpContextAccessor HttpContextAccessor
        {
            get
            {
                return this.ServiceProvider.GetService<IHttpContextAccessor>();
            }
        }

        private IRedisProvider RedisProvider
        {
            get
            {
                return this.ServiceProvider.GetService<IRedisProvider>();
            }
        }

        private IOptions<SiteConfiguration> _siteConfigurationAccessor;

        private SiteConfiguration SiteConfiguration
        {
            get
            {
                return _siteConfigurationAccessor.Value;
            }
        }

        private long _userID;

        private UserModel _user;

        public UserModel CurrentUser
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

        public bool IsAdmin
        {
            get
            {
                return this.IsLogin && this.SiteConfiguration.AdminUserList?.Contains(this.CurrentUser.UserName) == true;
            }
        }

        private long? _unreadMessages;

        public long UnreadMessages
        {
            get
            {
                if (!this.IsLogin)
                {
                    return 0;
                }

                if (_unreadMessages == null)
                {
                    var messageService = this.ServiceProvider.GetService<MessageService>();
                    _unreadMessages = messageService.QueryUnreadCount(this.CurrentUser.ID);
                }

                return _unreadMessages.Value;
            }
        }

        public SecurityManager(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this._siteConfigurationAccessor = serviceProvider.GetService<IOptions<SiteConfiguration>>();
        }

        public void ReloadUser()
        {
            if (_userID > 0)
            {
                var userService = this.ServiceProvider.GetService<UserService>();

                _user = userService.Get(Convert.ToInt64(_userID));
            }
        }

        public bool CanOperateTopic(Topic topic)
        {
            if (!this.IsLogin)
            {
                return false;
            }

            if (topic.CreateUser == this.CurrentUser.ID || this.IsAdmin)
            {
                return true;
            }

            return false;
        }

        public bool CanOperateTopic(TopicModel topic)
        {
            if (!this.IsLogin)
            {
                return false;
            }

            if (topic.CreateUser.ID == this.CurrentUser.ID || this.IsAdmin)
            {
                return true;
            }

            return false;
        }

        public bool CanOperateComment(Comment entity)
        {
            if (!this.IsLogin)
            {
                return false;
            }

            if (entity.CreateUser == this.CurrentUser.ID || this.IsAdmin)
            {
                return true;
            }

            return false;
        }

        public bool CanOperateComment(CommentModel comment)
        {
            if (!this.IsLogin)
            {
                return false;
            }

            if (comment.CreateUser.ID == this.CurrentUser.ID || this.IsAdmin)
            {
                return true;
            }

            return false;
        }

        private void LoadUser()
        {
            this.InitToken();
            if (string.IsNullOrWhiteSpace(this.Token))
            {
                return;
            }

            var redis = this.RedisProvider.GetDatabase();
            string tokenKey = RedisKeys.GetTokenCacheKey(this.Token);
            var id = redis.StringGet(tokenKey);

            if (id.HasValue)
            {
                _userID = Convert.ToInt64(id);

                var userService = this.ServiceProvider.GetService<UserService>();

                _user = userService.Get(Convert.ToInt64(id));
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

        public static void ClearToken(HttpContext context)
        {
            context.Response.Cookies.Append(TOKEN_KEY, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
        }
    }
}
