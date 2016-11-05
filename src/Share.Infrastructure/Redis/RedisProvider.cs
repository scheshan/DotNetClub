using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Share.Infrastructure.Redis
{
    public sealed class RedisProvider : IRedisProvider, IDisposable
    {
        private string _host;

        private int _port;

        private string _password;

        private int _db;

        private static readonly object _sync = new object();

        private ConnectionMultiplexer _pool;

        private ConnectionMultiplexer Pool
        {
            get
            {
                if (_pool == null)
                {
                    lock (_sync)
                    {
                        if (_pool == null)
                        {
                            Connect();
                        }
                    }
                }

                return _pool;
            }
        }

        public RedisProvider(string host, int port = 6379, string password = null, int db = -1)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentNullException(nameof(host));
            }

            _host = host;
            _port = port;
            _password = password;
            _db = db;
        }

        public IDatabase GetDatabase(int? db)
        {
            return this.Pool.GetDatabase(db ?? -1);
        }

        public IServer GetServer()
        {
            return this.Pool.GetServer(this.Pool.GetEndPoints().First());
        }

        private void Connect()
        {
            _pool = ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints =
                {
                    { _host, _port }
                },
                Password = _password,
                DefaultDatabase = _db
            });
        }

        public void Dispose()
        {
            if (this.Pool != null)
            {
                this.Pool.Dispose();
            }
        }
    }
}
