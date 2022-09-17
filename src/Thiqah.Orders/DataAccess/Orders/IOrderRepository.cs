using Microsoft.EntityFrameworkCore;
using Thiqah.Orders.Domain.Orders;

namespace Thiqah.Orders.DataAccess.Orders;

public interface IOrderRepository
{
    DbSet<Order> Orders { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
