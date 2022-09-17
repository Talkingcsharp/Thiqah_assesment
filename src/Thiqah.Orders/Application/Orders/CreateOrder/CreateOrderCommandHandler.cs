using MediatR;
using Thiqah.Orders.DataAccess.Orders;
using Thiqah.Orders.Domain.Orders;
using Thiqah.Orders.Infrastructure.Messages;
using Thiqah.Orders.Infrastructure.Request;
using Thiqah.Shared.EventModels.Users;
using Thiqah.Shared.Exceptions;
using Thiqah.Shared.Validation;

namespace Thiqah.Orders.Application.Orders.CreateOrder
{
    public sealed class CreateOrderCommandHandler : INotificationHandler<CreateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly RequestUserEventOperator _userEventOperator;
        private readonly UserCreateOrderEventPublisher _userCreateOrderEventPublisher;
        private readonly FluentValidator _fluentValidator;
        public CreateOrderCommandHandler(IOrderRepository orderRepository, RequestUserEventOperator userEventOperator, UserCreateOrderEventPublisher userCreateOrderEventPublisher, FluentValidator fluentValidator)
        {
            _orderRepository = orderRepository;
            _userEventOperator = userEventOperator;
            _userCreateOrderEventPublisher = userCreateOrderEventPublisher;
            _fluentValidator = fluentValidator;
        }

        public async Task Handle(CreateOrderCommand notification, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                IsDelivery = notification.IsDelivery,
                OrderDate = notification.OrderDate,
                TotalPrice = notification.TotalPrice,
                UserId = notification.UserId
            };

            _fluentValidator.Validate(order);

            order.OrderDate = order.OrderDate!.Value.ToUniversalTime();

            GetUserEventResponse? user;

            //This is not the best handling for user not found situation, however we can implement
            //a not found/error mechanism in the BaseMessage<T> class, ie: introduce two new properties
            //one IsSuccessd of boolean and one for the error of type Exception? 
            try
            {
                user = await _userEventOperator.GetUserAsync(order.UserId!.Value);
            }
            catch
            {
                throw new DataNotFoundException("User is not found");
            }

            await _orderRepository.Orders.AddAsync(order);

            await _orderRepository.SaveChangesAsync(cancellationToken);

            await _userCreateOrderEventPublisher.UserCreateOrder(order.UserId.Value);
        }
    }
}
