using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thiqah.Orders.Domain.Orders;

namespace Thiqah.Orders.EntityFramework.Mappers
{
    public sealed class OrdersMappers : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(x => !x.IsDeleted);
            builder.Ignore(x => x.Validator);
        }
    }
}
