namespace DotNetClub.Core.Redis
{
    public class RedisOptions
    {
        /// <summary>
        /// Redis end points, such as "{host or ip}:{port}"
        /// </summary>
        public string[] EndPoints { get; set; }

        /// <summary>
        /// Redis password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Default redis database
        /// </summary>
        public int Db { get; set; }
    }
}
