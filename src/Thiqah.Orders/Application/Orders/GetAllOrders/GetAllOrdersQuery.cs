using MediatR;
using Thiqah.Orders.Domain.Orders;

namespace Thiqah.Orders.Application.Orders.GetAllOrders;

public record GetAllOrdersQuery: IRequest<List<Order>>;
