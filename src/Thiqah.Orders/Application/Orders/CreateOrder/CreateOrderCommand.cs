using MediatR;

namespace Thiqah.Orders.Application.Orders.CreateOrder
{
    public record CreateOrderCommand: INotification
    {
        public DateTime? OrderDate { get; set; }
        public int? UserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool IsDelivery { get; set; }
    }
}
