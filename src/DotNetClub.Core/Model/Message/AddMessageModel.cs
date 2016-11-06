using DotNetClub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.Message
{
    public class AddMessageModel
    {
        public MessageType Type { get; set; }

        public long? Topic { get; set; }

        public long? Comment { get; set; }

        public long? FromUser { get; set; }

        public long ToUser { get; set; }
    }
}
