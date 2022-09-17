using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Thiqah.Shared.Context;
using Thiqah.Shared.EventModels.Users;
using Thiqah.Shared.Messaging;
using Thiqah.Users.Application.Users;

namespace Thiqah.Users.Infrastructure.Messages;

public sealed class UserCreatedOrderEventHandler : IHostedService
{


    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMessenger _messenger;
    public UserCreatedOrderEventHandler(IServiceScopeFactory scopeFactory, IMessenger messenger)
    {
        _scopeFactory = scopeFactory;
        _messenger = messenger;
    }

    public async Task RequestReceived(UserCreatedOrderEvent input)
    {
        using(var scope = _scopeFactory.CreateScope())
        {
            try
            {
                var activecontext = scope.ServiceProvider.GetService<ActiveContext>();
                activecontext!.FromActiveContext(input.Header.ActiveContext!);

                var command = scope.ServiceProvider.GetService<IUsersCommand>();
                if (command is null)
                {
                    return;
                }

                await command.IncreaseUserOrders(input.UserId);
            }
            catch
            {
                //Log the exception without breaking the app or we will lose the consumer.
            }
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _messenger.Subscribe(new UserCreatedOrderEvent(), RequestReceived);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
