using Thiqah.Shared.Context;
using Thiqah.Shared.EventModels.Users;
using Thiqah.Shared.Messaging;

namespace Thiqah.Orders.Infrastructure.Request;

public sealed class RequestUserEventOperator
{
    private readonly IMessenger _messenger;
    private readonly ActiveContext _activeContext;
    public RequestUserEventOperator(IMessenger messenger, ActiveContext activeContext)
    {
        _messenger = messenger;
        _activeContext = activeContext;
    }

    public async Task<GetUserEventResponse?> GetUserAsync(int id)
    {
        var request = new GetUserEvent
        {
            Id = id
        };

        request.Header.ActiveContext = _activeContext;

        var response = await _messenger.Request<GetUserEvent, GetUserEventResponse>(request, 20);

        return response;
    }
}
