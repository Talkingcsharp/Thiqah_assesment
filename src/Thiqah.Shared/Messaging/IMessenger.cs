using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thiqah.Shared.Messaging.Base;

namespace Thiqah.Shared.Messaging
{
    public interface IMessenger
    {
        Task Publish<T>(BaseMessage<T> messageObject);
        Task Subscribe<T>(BaseMessage<T> messageObject, Func<T, Task> received);
        Task<TResponse> Request<TRequest, TResponse>(BaseMessage<TRequest> messageObject, int waitInSeconds = 5);
    }
}
