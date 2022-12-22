using Contracts.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Customer.API.Repositories;

public class CustomerRepository : RepositoryQueryBase<Entities.Customer, int, CustomerContext>
    , ICustomerRepository
{
    public CustomerRepository(CustomerContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Entities.Customer>> GetCustomers()
    {
        return await FindAll().ToListAsync();
    }

    public async Task<Entities.Customer> GetCustomer(int id)
    {
        return await GetByIdAsync(id);
    }
    
    // public async Task CreateCustomer(Entities.Customer customer)
    // {
    //     CreateAsync(customer);
    // }
    //
    // public async Task UpdateCustomer(Entities.Customer customer)
    // {
    //     await UpdateAsync(customer);
    // }
    //
    // public async Task DeleteCustomer(int id)
    // {
    //     var product = await GetByIdAsync(id);
    //     if(product != null)
    //         await DeleteAsync(product);
    // }
}