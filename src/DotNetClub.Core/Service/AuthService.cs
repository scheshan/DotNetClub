using DotNetClub.Core.Data;
using DotNetClub.Domain.Entity;
using DotNetClub.Core.Model;
using DotNetClub.Core.Utility;
using Microsoft.Extensions.Configuration;
using Share.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Share.Infrastructure.Model;
using Share.Infrastructure.UnitOfWork;
using DotNetClub.Core.Model.Auth;

namespace DotNetClub.Core.Service
{
    public class AuthService : ServiceBase
    {        
        public AuthService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public async Task<Result<string>> Register(RegisterModel model)
        {
            if (!this.SiteConfiguration.AllowRegister)
            {
                return Result<string>.ErrorResult("站点目前禁止注册");
            }

            using (var uw = this.CreateUnitOfWork())
            {
                if (await uw.ExistAsync<User>(t => t.UserName == model.UserName))
                {
                    return Result<string>.ErrorResult("用户名已被注册");
                }
                if (await uw.ExistAsync<User>(t => t.Email == model.Email))
                {
                    return Result<string>.ErrorResult("邮箱已被注册");
                }

                var entity = new User
                {
                    CreateDate = DateTime.Now,
                    Email = model.Email,
                    UserName = model.UserName,
                    Password = EncryptHelper.EncryptMD5(model.Password)
                };

                if (this.SiteConfiguration.AdminUserList?.Contains(model.UserName, StringComparer.CurrentCultureIgnoreCase) == true)
                {
                    entity.Status = Domain.Enums.UserStatus.Active;
                }
                else if (this.SiteConfiguration.VerifyRegisterUser)
                {
                    entity.Status = Domain.Enums.UserStatus.Verifying;
                }
                else
                {
                    entity.Status = Domain.Enums.UserStatus.Active;
                }

                await uw.InsertAsync(entity);

                string token = this.GenerateAndStoreToken(entity.ID, false);

                return Result.SuccessResult(token);
            }            
        }

        public async Task<Result<string>> Login(LoginModel model)
        {
            string password = EncryptHelper.EncryptMD5(model.Password);

            using (var uw = this.CreateUnitOfWork())
            {
                var user = await uw.GetAsync<User>(t => t.Email == model.Email && t.Password == password);

                if (user == null)
                {
                    return Result<string>.ErrorResult("邮箱或密码不匹配");
                }
                if (user.Status == Domain.Enums.UserStatus.Verifying)
                {
                    return Result<string>.ErrorResult("您还未通过管理员审核, 请耐心等待");
                }
                if (user.Status == Domain.Enums.UserStatus.Deny)
                {
                    return Result<string>.ErrorResult("您已经被禁止登录");
                }

                string token = this.GenerateAndStoreToken(user.ID, model.RememberPassword);

                return Result.SuccessResult(token);
            }
        }

        public async Task<bool> IsEmailRegistered(string email)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                return await uw.ExistAsync<User>(t => t.Email == email);
            }
        }

        public async Task<bool> IsUserNameRegistered(string userName)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                return await uw.ExistAsync<User>(t => t.UserName == userName);
            }
        }

        public void LogOut()
        {
            if (this.SecurityManager.Token != null)
            {
                var database = this.RedisProvider.GetDatabase();
                string key = $"{Domain.Consts.RedisKeys.TokenPrefix}{this.SecurityManager.Token}";
                database.KeyDelete(key);
            }
        }

        private string GenerateAndStoreToken(long userID, bool rememberPassword)
        {
            var database = this.RedisProvider.GetDatabase();
            string token = EncryptHelper.EncryptMD5(Guid.NewGuid().ToString());
            string key = $"{Domain.Consts.RedisKeys.TokenPrefix}{token}";
            database.StringSet(key, userID.ToString());
            if (!rememberPassword)
            {
                database.KeyExpire(key, DateTime.Now.AddDays(7));
            }
            else
            {
                database.KeyExpire(key, DateTime.Now.AddDays(30));
            }

            return token;
        }

        private string ComputeNewToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
