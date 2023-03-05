using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Serilog;
using Shared.SeedWord;

namespace Ordering.Application.Features.V1.Orders;

public class DeleteOrderByDocumentNoHandler : IRequestHandler<DeleteOrderByDocumentNoCommand, ApiSuccessResult<bool>>
{
    private readonly IOrderRepository _repository;
    private readonly ILogger _logger;
    private const string MethodName = "DeleteOrderByDocumentNoHandler";

    public DeleteOrderByDocumentNoHandler(IOrderRepository repository, ILogger logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiSuccessResult<bool>> Handle(DeleteOrderByDocumentNoCommand request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: {MethodName}");

        var order = await _repository.GetOrderByDocumentNo(request.DocumentNo);

        if (order == null) throw new NotFoundException(nameof(order), request.DocumentNo);
        _repository.Delete(order);

        order.DeletedOrder();

        await _repository.SaveChangesAsync();

        _logger.Information($"END: {MethodName}");

        return new ApiSuccessResult<bool>(true);
    }
}
