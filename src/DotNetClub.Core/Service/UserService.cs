using DotNetClub.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Share.Infrastructure.UnitOfWork;
using DotNetClub.Core.Model.User;
using Share.Infrastructure.Model;
using Share.Infrastructure.Utilities;
using Share.Infrastructure.Redis;
using DotNetClub.Domain.Consts;
using Share.Infrastructure.Extensions;

namespace DotNetClub.Core.Service
{
    public class UserService : ServiceBase
    {
        public UserService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public async Task<Result> EditUserInfo(long id, EditUserInfoModel model)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var user = await uw.GetAsync<User>(t => t.ID == id);

                if (user == null)
                {
                    return Result.ErrorResult("该用户不存在");
                }

                user.WebSite = model.WebSite;
                user.Location = model.Location;
                user.Signature = model.Signature;

                await uw.UpdateAsync(user);

                var redis = this.RedisProvider.GetDatabase();
                redis.JsonHashSet(RedisKeys.User, user.ID, user);

                this.SecurityManager.ReloadUser();

                return Result.SuccessResult();
            }
        }

        public async Task<Result> EditPassword(long id, EditPasswordModel model)
        {
            string oldPassword = EncryptHelper.EncryptMD5(model.OldPassword);
            string newPassword = EncryptHelper.EncryptMD5(model.NewPassword);

            using (var uw = this.CreateUnitOfWork())
            {
                var user = await uw.GetAsync<User>(t => t.ID == id && t.Password == oldPassword);
                if (user == null)
                {
                    return Result.ErrorResult("密码错误");
                }

                user.Password = newPassword;
                await uw.UpdateAsync(user);

                var redis = this.RedisProvider.GetDatabase();
                redis.JsonHashSet(RedisKeys.User, user.ID, user);

                return Result.SuccessResult();
            }
        }

        public async Task<UserModel> Get(string userName)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var user = await uw.GetAsync<User>(t => t.UserName == userName);

                if (user == null)
                {
                    return null;
                }

                return this.Transform(user).First();
            }
        }

        public UserModel Get(long id)
        {
            var redis = this.RedisProvider.GetDatabase();

            var user = redis.JsonHashGet<User>(RedisKeys.User, id);

            if (user == null)
            {
                return null;
            }

            return this.Transform(user).First();
        }

        private List<UserModel> Transform(params User[] entityList)
        {
            if (entityList.IsEmptyCollection())
            {
                return new List<UserModel>();
            }

            return entityList.Select(t => new UserModel
            {
                CreateDate = t.CreateDate,
                Email = t.Email,
                ID = t.ID,
                Location = t.Location,
                Signature = t.Signature,
                Status = t.Status,
                UserName = t.UserName,
                WebSite = t.WebSite
            }).ToList();
        }
    }
}
