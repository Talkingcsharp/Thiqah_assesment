using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thiqah.Shared.Context;

namespace Thiqah.Shared.Messaging.Base
{
    public sealed class BaseMessageHeader
    {
        public ActiveContext? ActiveContext { get; set; }
        public string? Topic { get; set; }

        public string? Queue { get; set; }
        public string? ReplyToAddress { get; set; }
        public string? ReplytToCorelationId { get; set; }
    }
}
