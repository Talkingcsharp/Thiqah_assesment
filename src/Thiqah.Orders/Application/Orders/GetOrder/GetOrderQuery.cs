using MediatR;
using Thiqah.Orders.Domain.Orders;

namespace Thiqah.Orders.Application.Orders.GetOrder;

public record GetOrderQuery:IRequest<Order>
{
    public int Id { get; set; }
}
