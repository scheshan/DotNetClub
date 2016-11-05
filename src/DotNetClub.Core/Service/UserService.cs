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

                return Result.SuccessResult();
            }
        }

        public async Task<Result> EditPassword(long id, string oldPassword, string newPassword)
        {
            oldPassword = EncryptHelper.EncryptMD5(oldPassword);
            newPassword = EncryptHelper.EncryptMD5(newPassword);

            using (var uw = this.CreateUnitOfWork())
            {
                var user = await uw.GetAsync<User>(t => t.ID == id && t.Password == oldPassword);
                if (user == null)
                {
                    return Result.ErrorResult("密码错误");
                }

                user.Password = newPassword;
                await uw.UpdateAsync(user);

                return Result.SuccessResult();
            }
        }

        public async Task<User> Get(string userName)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var user = await uw.GetAsync<User>(t => t.UserName == userName);
                return user;
            }
        }

        public async Task<User> Get(int id)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var user = await uw.GetAsync<User>(t => t.ID == id);
                return user;
            }
        }
    }
}
