using MediatR;

namespace Ordering.Application.Features.V1.Orders;

public class DeleteOrderCommand : IRequest<bool>
{
    public int Id { get; private set; }
    public DeleteOrderCommand(int id)
    {
        Id = id;
    }
}
