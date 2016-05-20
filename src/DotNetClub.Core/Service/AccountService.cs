using DotNetClub.Core.Data;
using DotNetClub.Core.Entity;
using DotNetClub.Core.Model;
using DotNetClub.Core.Model.Account;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DotNetClub.Core.Service
{
    public class AccountService
    {
        private ClubContext DbContext { get; set; }

        private IConfiguration Configuration { get; set; }

        public AccountService(ClubContext dbContext, IConfiguration configuration)
        {
            this.DbContext = dbContext;
            this.Configuration = configuration;
        }

        public async Task<RegisterResult> Register(string userName, string password, string email)
        {
            var result = new RegisterResult();

            if (this.DbContext.Users.Any(t => t.UserName == userName))
            {
                result.ErrorCode = RegisterResult.RegisterErrorCode.UserNameExist;
                return result;
            }
            if (this.DbContext.Users.Any(t => t.Email == email))
            {
                result.ErrorCode = RegisterResult.RegisterErrorCode.EmailExist;
                return result;
            }

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var entity = new User
            {
                Active = true,
                CreateDate = DateTime.Now,
                DisplayName = userName,
                Email = email,
                IsBlock = false,
                UserName = userName,
                Salt = Convert.ToBase64String(salt),
                Password = this.ComputePassword(salt, password)
            };

            this.DbContext.Add(entity);
            await this.DbContext.SaveChangesAsync();

            result.Success = true;

            return result;
        }

        public async Task<LoginResult> Login(string userName, string password)
        {
            var result = new LoginResult();

            var user = this.DbContext.Users.SingleOrDefault(t => t.UserName == userName);

            if (user == null)
            {
                result.ErrorCode = LoginResult.LoginErrorCode.UserNotExist;
                return result;
            }
            if (!user.Active)
            {
                result.ErrorCode = LoginResult.LoginErrorCode.UserNotActive;
                return result;
            }
            if (user.IsBlock)
            {
                result.ErrorCode = LoginResult.LoginErrorCode.UserIsBlocked;
                return result;
            }

            byte[] salt = Convert.FromBase64String(user.Salt);
            password = this.ComputePassword(salt, password);

            if (password != user.Password)
            {
                result.ErrorCode = LoginResult.LoginErrorCode.InvalidPassword;
                return result;
            }

            result.Token = user.Token = this.ComputeNewToken();
            await this.DbContext.SaveChangesAsync();

            result.Success = true;
            return result;
        }

        private string ComputePassword(byte[] salt, string password)
        {
            string result = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return result;
        }

        private string ComputeNewToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
