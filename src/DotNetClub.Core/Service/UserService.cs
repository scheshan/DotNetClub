using DotNetClub.Core.Entity;
using DotNetClub.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotNetClub.Core.Service
{
    public class UserService
    {
        private Data.ClubContext DbContext { get; set; }

        public UserService(Data.ClubContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public async Task EditUserInfo(int id, string webSite, string location, string signature)
        {
            var user = this.DbContext.Users.SingleOrDefault(t => t.ID == id);
            if (user != null)
            {
                user.WebSite = webSite;
                user.Location = location;
                user.Signature = signature;

                await this.DbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> EditPassword(int id, string oldPassword, string newPassword)
        {
            var user = this.DbContext.Users.SingleOrDefault(t => t.ID == id);
            if (user == null)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(user.Salt);

            oldPassword = EncryptHelper.Encrypt(salt, oldPassword);

            if (oldPassword != user.Password)
            {
                return false;
            }

            newPassword = EncryptHelper.Encrypt(salt, newPassword);
            user.Password = newPassword;
            await this.DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<User> Get(string userName)
        {
            return await this.DbContext.Users.SingleOrDefaultAsync(t => t.UserName == userName);
        }

        public async Task<User> Get(int id)
        {
            return await this.DbContext.Users.SingleOrDefaultAsync(t => t.ID == id);
        }
    }
}
