using DotNetClub.Core.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core
{
    public static class IServiceCollectionExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<CategoryService>()
                .AddScoped<AccountService>()
                .AddScoped<UserService>()
                .AddScoped<TopicService>()
                .AddScoped<CommentService>()
                .AddScoped<UserVoteService>();

            services.AddScoped<ClientManager>();
        }
    }
}
