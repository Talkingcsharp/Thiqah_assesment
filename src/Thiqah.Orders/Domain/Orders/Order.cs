using FluentValidation;
using Thiqah.Shared.Domain;
using Thiqah.Shared.Validation;

namespace Thiqah.Orders.Domain.Orders;

public sealed class Order : BaseDomain, IValidationModel<Order>
{
    public AbstractValidator<Order> Validator => new OrderValidator();

    public DateTime?  OrderDate { get; set; }
    public int? UserId { get; set; }
    public decimal? TotalPrice { get; set; }
    public bool IsDelivery { get; set; }
}

public sealed class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(x => x.OrderDate).NotNull();
        RuleFor(x => x.UserId).NotNull().GreaterThan(0);
        RuleFor(x => x.TotalPrice).GreaterThan(0).NotNull();
    }
}
