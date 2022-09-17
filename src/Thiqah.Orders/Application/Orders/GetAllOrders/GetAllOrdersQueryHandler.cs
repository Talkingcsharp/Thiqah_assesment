using MediatR;
using Microsoft.EntityFrameworkCore;
using Thiqah.Orders.DataAccess.Orders;
using Thiqah.Orders.Domain.Orders;

namespace Thiqah.Orders.Application.Orders.GetAllOrders;

public sealed class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<Order>>
{
    private readonly IOrderQueryRepository _orderQueryRepository;

    public GetAllOrdersQueryHandler(IOrderQueryRepository orderQueryRepository)
    {
        _orderQueryRepository = orderQueryRepository;
    }

    public async Task<List<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _orderQueryRepository.OrdersQueryable.ToListAsync();
    }
}
