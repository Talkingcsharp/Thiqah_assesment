using Thiqah.Shared.Context;
using Thiqah.Shared.EventModels.Users;
using Thiqah.Shared.Messaging;
using Thiqah.Shared.Messaging.Base;
using Thiqah.Users.Application.Users;

namespace Thiqah.Users.Infrastructure.Messages;

public sealed class GetUserEventHandler: IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMessenger _messenger;

    public GetUserEventHandler(IServiceScopeFactory scopeFactory, IMessenger messenger)
    {
        _scopeFactory = scopeFactory;
        _messenger = messenger;
    }

    public async Task RequestReceived(GetUserEvent input)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            try
            {
                var activecontext = scope.ServiceProvider.GetService<ActiveContext>();
                activecontext!.FromActiveContext(input.Header.ActiveContext!);

                var query = scope.ServiceProvider.GetService<IUsersQuery>();
                if (query is null)
                {
                    return;
                }

                var user = await query.GetUser(input.Id);

                var response = new GetUserEventResponse
                {
                    Age = user.Age,
                    Email = user.Email,
                    IsMale = user.IsMale,
                    Name = user.Name,
                    OrdersCount = user.OrdersCount,
                    UserName = user.UserName
                };

                response.Header.ReplyToAddress = input.Header.ReplyToAddress;
                response.Header.ReplytToCorelationId = input.Header.ReplytToCorelationId;
                response.Header.Topic = string.Empty;

                await _messenger.Publish(response);
            }
            catch
            {
                //Log the exception without breaking the app or we will lose the consumer.
            }
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _messenger.Subscribe(new GetUserEvent(), RequestReceived);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
