using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Thiqah.Orders.Application.Orders.CreateOrder;
using Thiqah.Orders.Application.Orders.GetAllOrders;
using Thiqah.Orders.Application.Orders.GetOrder;
using Thiqah.Orders.Controllers.ViewModels;
using Thiqah.Orders.Domain.Orders;
using Thiqah.Shared.Exceptions;

namespace Thiqah.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/api/order/{id:int}")]
        [ProducesResponseType(typeof(OrderViewModel),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ExceptionViewModel),404)]
        public async Task<ActionResult<OrderViewModel>> GetOrder([FromRoute] int id)
        {
            return OrderViewModel.FromOrder(await _mediator.Send<Order>(new GetOrderQuery()
            {
                Id = id
            }));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<OrderViewModel>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<List<OrderViewModel>>> GetOrders()
        {
            var orders = await _mediator.Send<List<Order>>(new GetAllOrdersQuery());
            if (orders is null || orders.Count == 0)
            {
                return NoContent();
            }

            return Ok(orders.Select(s => OrderViewModel.FromOrder(s)).ToList());
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderCommand createOrderCommand)
        {
            await _mediator.Publish<CreateOrderCommand>(createOrderCommand);

            return Created("", null);
        }
    }
}
