using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.Configuration
{
    public class RedisConfiguration
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string Password { get; set; }

        public int Db { get; set; }
    }
}
