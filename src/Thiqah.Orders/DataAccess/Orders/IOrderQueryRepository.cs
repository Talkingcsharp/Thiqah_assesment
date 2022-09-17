using Thiqah.Orders.Domain.Orders;

namespace Thiqah.Orders.DataAccess.Orders;

public interface IOrderQueryRepository
{
 IQueryable<Order> OrdersQueryable { get; }
}
