using MediatR;
using Shared.SeedWord;

namespace Ordering.Application.Features.V1.Orders;

public class DeleteOrderByDocumentNoCommand : IRequest<ApiSuccessResult<bool>>
{
    public string DocumentNo { get; private set; }
    public DeleteOrderByDocumentNoCommand(string documentNo)
    {
        DocumentNo = documentNo;
    }
}
