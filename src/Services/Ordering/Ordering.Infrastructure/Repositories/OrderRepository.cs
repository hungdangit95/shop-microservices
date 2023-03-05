using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using Ordering.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBase<Order, int, OrderContext>, IOrderRepository
{
    public OrderRepository(OrderContext context, IUnitOfWork<OrderContext> unitOfWork) : base(context, unitOfWork)
    {
    }

    public async Task<IEnumerable<Order>> GetOrdersByUsername(string username)
    {
        return await FindByCondition(x => x.Username.Equals(username)).ToListAsync();
    }

    public async Task CreateOrder(Order request)
    {
        await CreateAsync(request);
    }

    public async Task UpdateOrder(Order request)
    {
        await UpdateAsync(request);
    }

    public async Task DeleteOrder(int id)
    {
        var order = await this.GetByIdAsync(id);
        await DeleteAsync(order);
    }

    public async Task<Order> CreateOrderMQ(Order order)
    {
        await CreateAsync(order);
        return order;   
    }

    public async Task<Order?> GetOrderByDocumentNo(string documentNo)
    {
        return await FindByCondition(x => x.DocumentNo.ToString().Equals(documentNo)).FirstOrDefaultAsync();
    }
}
