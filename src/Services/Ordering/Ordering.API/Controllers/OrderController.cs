using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Features.V1.Orders;
using Shared.Dtos.Order;
using Shared.SeedWord;
using System.ComponentModel.DataAnnotations;
using System.Net;
using OrderDto = Ordering.Application.Common.Models.OrderDto;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public OrderController(IMediator mediator, IOrderRepository repository, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentException(nameof(mediator));
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        private static class RouteNames
        {
            public const string GetOrders = nameof(GetOrders);
            public const string CreateOrder = nameof(CreateOrder);
            public const string UpdateOrder = nameof(UpdateOrder);
            public const string DeleteOrder = nameof(DeleteOrder);
            public const string GetOrderById = nameof(GetOrderById);
            public const string DeleteOrderByDocumentNo = nameof(DeleteOrderByDocumentNo);
        }

        [HttpGet("{username}", Name = RouteNames.GetOrders)]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUsername([Required] string username)
        {
            var query = new GetOrdersQuery(username);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = RouteNames.GetOrderById)]
        [ProducesResponseType(typeof(OrderDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderDto>> GetOrderById([Required] int id)
        {
            var query = new GetOrderByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost(Name = RouteNames.CreateOrder)]
        [ProducesResponseType(typeof(ApiResult<int>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<OrderDto>>> CreateOrder([FromBody] CreateOrderDto model)
        {
            var command = _mapper.Map<CreateOrderCommand>(model);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id:int}",Name = RouteNames.UpdateOrder)]
        [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<OrderDto>>> UpdateOrder([Required] int id, [FromBody] UpdateOrderCommand command)
        {
            command.SetId(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id:int}", Name = RouteNames.DeleteOrder)]
        public async Task<ActionResult<bool>> DeleteOrder([Required] int id)
        {
            var command = new DeleteOrderCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("document-no/{documentNo}", Name = RouteNames.DeleteOrderByDocumentNo)]
        public async Task<ActionResult<bool>> DeleteOrderByDocumentNo([Required] string documentNo)
        {
            var command = new DeleteOrderByDocumentNoCommand(documentNo);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
