using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thiqah.Shared.Messaging.Base;

namespace Thiqah.Shared.EventModels.Users;

public sealed class UserCreatedOrderEvent: BaseMessage<UserCreatedOrderEvent>
{
    public int UserId { get; set; }
}
