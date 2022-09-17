using Thiqah.Orders.Domain.Orders;

namespace Thiqah.Orders.Controllers.ViewModels
{
    public sealed class OrderViewModel
    {
        public DateTime? OrderDate { get; set; }
        public int? UserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool IsDelivery { get; set; }

        public static OrderViewModel FromOrder(Order order)
        {
            return new OrderViewModel
            {
                IsDelivery = order.IsDelivery,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                UserId = order.UserId
            };
        }
    }
}
