using MediatR;
using Microsoft.EntityFrameworkCore;
using Thiqah.Orders.DataAccess.Orders;
using Thiqah.Orders.Domain.Orders;
using Thiqah.Shared.Exceptions;

namespace Thiqah.Orders.Application.Orders.GetOrder;

public sealed class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Order>
{
    
    private readonly IOrderQueryRepository _orderQueryRepository;

    public GetOrderQueryHandler(IOrderQueryRepository orderQueryRepository)
    {
        _orderQueryRepository = orderQueryRepository;
    }

    public async Task<Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderQueryRepository.OrdersQueryable.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(order is null)
        {
            throw new DataNotFoundException();
        }

        return order;
    }
}
