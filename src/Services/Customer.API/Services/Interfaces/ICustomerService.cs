using Shared.Dtos.Customer;

namespace Customer.API.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetCustomers();
    Task<CustomerDto> GetCustomer(int id);
    //Task CreateCustomer(CreateCustomerDto customerDto);
    //Task UpdateCustomer(int id, UpdateCustomerDto customerDto);
    //Task DeleteCustomer(int id);
}