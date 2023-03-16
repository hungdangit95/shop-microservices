using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using MassTransit;
using AutoMapper;
//using EventBus.Messages.IntegrationEvents.Events;
//using Basket.API.GrpcServices;
//using Basket.API.Services.Interfaces;
using Shared.Dtos.Basket;
using EventBus.Messages.IntegrationEvents.Events;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketsController : Controller
{
    private readonly IBasketRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;
    // private readonly StockItemGrpcService _stockItemGrpcService;

    public BasketsController(IBasketRepository repository, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("{username}", Name = "GetBasket")]
    [ProducesResponseType(typeof(CartDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CartDto>> GetBasketByUsername([Required] string username)
    {
        var cart = await _repository.GetBasketByUsername(username);
        var result = cart == null ? new CartDto(username) : _mapper.Map<CartDto>((Cart)cart);
        return Ok(result);
    }

    [HttpPost(Name = "UpdateBasket")]
    [ProducesResponseType(typeof(CartDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CartDto>> UpdateBasket([FromBody] CartDto model)
    {
        //foreach(var item in model.Items)
        //{
        //var stock = await _stockItemGrpcService.GetStock(item.ItemNo);
        // item.SetAvailableQuantity(stock.Quantity);
        //}    

        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(1))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        var cart = _mapper.Map<Cart>(model);

        var updatedCart = await _repository.UpdateBasket(cart, options);

        var result = _mapper.Map<CartDto>(updatedCart);
        return Ok(result);
    }

    [HttpDelete("{username}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteBasket([Required] string username)
    {
        var result = await _repository.DeleteBasketFromUsername(username);
        return Ok(result);
    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        var basket = await _repository.GetBasketByUsername(basketCheckout.Username);

        if (basket == null || !basket.Items.Any()) return NotFound();

        var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);

        eventMessage.TotalPrice = basketCheckout.TotalPrice;
        await _publishEndpoint.Publish(eventMessage);

        await _repository.DeleteBasketFromUsername(basketCheckout.Username);

        return Accepted();
    }
}