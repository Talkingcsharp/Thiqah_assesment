using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thiqah.Shared.Messaging.Base;

namespace Thiqah.Shared.EventModels.Users;

public sealed class GetUserEventResponse: BaseMessage<GetUserEventResponse>
{
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public bool IsMale { get; set; }
    public int Age { get; set; }
    public int OrdersCount { get; set; }
}
