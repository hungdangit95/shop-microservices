using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Serilog;

namespace Ordering.Application.Features.V1.Orders
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger _logger;
        private const string MethodName = "DeleteOrderHandler";

        public DeleteOrderHandler(IOrderRepository repository, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName}");

            var order = await _repository.GetByIdAsync(request.Id);

            if (order == null) throw new NotFoundException(nameof(order), request.Id);
            _repository.Delete(order);

            order.DeletedOrder();

            await _repository.SaveChangesAsync();

            _logger.Information($"END: {MethodName}");

            return true;
        }
    }
}
