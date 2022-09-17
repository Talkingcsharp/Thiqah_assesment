using Thiqah.Shared.Context;
using Thiqah.Shared.EventModels.Users;
using Thiqah.Shared.Messaging;

namespace Thiqah.Orders.Infrastructure.Messages
{
    public sealed class UserCreateOrderEventPublisher
    {
        private readonly IMessenger _messenger;
        private readonly ActiveContext _activeContext;
        public UserCreateOrderEventPublisher(IMessenger messenger, ActiveContext activeContext)
        {
            _messenger = messenger;
            _activeContext = activeContext;
        }

        public async Task UserCreateOrder(int userId)
        {
            var message = new UserCreatedOrderEvent
            {
                UserId = userId
            };
            message.Header.ActiveContext = _activeContext;
            await _messenger.Publish(message);
        }
    }
}
