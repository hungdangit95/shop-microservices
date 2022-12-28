using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using ILogger = Serilog.ILogger;
using StackExchange.Redis;
using Infrastructures.Extensions;
//using Basket.API.Services;
//using Basket.API.Services.Interfaces;
//using Shared.Dtos.ScheduledJob;
//using Infrastructure.Extensions;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    //private readonly IDistributedCache _redisCacheService;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger _logger;
    private readonly IDatabase _redisDb;
    //private readonly BackgroundJobHttpService _backgroundJobHttp;
    //private readonly IEmailTemplateService _emailTemplateService;

    public BasketRepository(ILogger logger, IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _redisDb = _connectionMultiplexer.GetDatabase();
        _logger = logger;
        //_backgroundJobHttp = backgroundJobHttp;
        //_emailTemplateService = emailTemplateService;
    }

    public async Task<Cart?> GetBasketByUsername(string username)
    {
        _logger.Information($"BEGIN: GetBasketByUsername {username}");
        var basket = await _redisDb.GetAsync<Cart>(username);
        _logger.Information($"END: GetBasketByUsername {username}");

        return basket;
    }

    public async Task<Cart?> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
    {
        await DeleteReminderCheckoutOrder(cart.Username);
        _logger.Information($"BEGIN: UpdateBasket for {cart.Username}");
        if (options != null)
        {
            await _redisDb.JsonSetAsync(cart.Username, cart);
            //  await _redisCacheService.SetStringAsync(cart.Username, _serializeService.Serialize(cart), options);
        }
        else
        {
            await _redisDb.JsonSetAsync(cart.Username, cart);
            //await _redisCacheService.SetStringAsync(cart.Username, _serializeService.Serialize(cart));
        }
        _logger.Information($"END: UpdateBasket for {cart.Username}");

        //try
        //{
        //    await TriggerSendEmailReminderCheckout(cart);
        //}
        //catch (Exception ex)
        //{
        //    _logger.Error(ex.Message);
        //}

        return await GetBasketByUsername(cart.Username);
    }

    private async Task TriggerSendEmailReminderCheckout(Cart cart)
    {
        // var emailTemplate = _emailTemplateService.GenerateReminderCheckoutOrderEmail(cart.Username);

        //var model = new ReminderCheckoutOrderDto(cart.EmailAddress, "Reminder checkout", emailTemplate, DateTimeOffset.UtcNow.AddSeconds(30));

        //try
        //{
        //    var jobId = await _backgroundJobHttp.SendEmailReminderCheckout(model);
        //    if (!string.IsNullOrEmpty(jobId))
        //    {
        //        cart.JobId = jobId;
        //        await _redisCacheService.SetStringAsync(cart.Username, _serializeService.Serialize(cart));
        //    }
        //}
        //catch (Exception ex)
        //{
        //    _logger.Error(ex.Message);
        //}
    }

    private async Task DeleteReminderCheckoutOrder(string username)
    {
        var cart = await GetBasketByUsername(username);

        if (cart == null || string.IsNullOrEmpty(cart.JobId)) return;

        var jobId = cart.JobId;
        // _backgroundJobHttp.DeleteReminderCheckoutOrder(jobId);
        _logger.Information($"DeleteReminderCheckoutOrder: Deleted JobId {jobId}");
    }

    public async Task<bool> DeleteBasketFromUsername(string username)
    {
        await DeleteReminderCheckoutOrder(username);
        try
        {
            _logger.Information($"BEGIN: DeleteBasketFromUsername {username}");
            await _redisDb.HDeleteAsync(username);
            _logger.Information($"END: DeleteBasketFromUsername {username}");

            return true;
        }
        catch (Exception e)
        {
            _logger.Error($"Error DeleteBasketFromUsername {e.Message}");
            throw;
        }

    }
}