using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, ApiResult<OrderDto>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly ILogger _logger;
    private const string MethodName = "CreateOrderHandler";
    public CreateOrderHandler(IMapper mapper, IOrderRepository repository, ILogger logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: {MethodName}");

        var order = _mapper.Map<Order>(request);
        await _repository.CreateOrder((Order)order);
        
        order.AddedOrder();

        await _repository.SaveChangesAsync();

        _logger.Information($"END: {MethodName}");

        return new ApiSuccessResult<OrderDto>(
            _mapper.Map<OrderDto>(order));
    }
}
