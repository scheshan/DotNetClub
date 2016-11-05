using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetClub.Domain.Entity;
using DotNetClub.Core.Model.User;
using DotNetClub.Core.Model.Topic;

namespace DotNetClub.Core
{
    public sealed class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(configuration =>
            {
                configuration.CreateMap<User, UserBasicModel>();
                configuration.CreateMap<User, UserModel>();
                configuration.CreateMap<Topic, TopicModel>();
                configuration.CreateMap<Topic, TopicBasicModel>();
            });
        }
    }
}
